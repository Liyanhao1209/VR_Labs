using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


internal struct LevelRequireInfor
{
    internal int level; // 第几关
    internal float time; // 这关倒计时多长时间
    internal int target; // 目标过关收益
    internal float interval; // 产生顾客间隔
}

public class GameControl
{

    //所有关卡信息
    internal static LevelRequireInfor[] allLevelRequire;
    private GameControl() { }

    //当前所有关卡信息
    private static LevelRequireInfor currentLevelInfor;
    private static int incomeCount;
    private static bool levelRunning;
    public int baseTime = 50;

    public static void DataInit()
    {
        //构建需求图
        DemandControl.BuildDemandGraphic();
        allLevelRequire = new LevelRequireInfor[]
        {
            new LevelRequireInfor(){level =1, time = 50,target = 10,interval = 20},
            new LevelRequireInfor(){level =2, time = 80,target = 30,interval =15},
            new LevelRequireInfor(){level =3, time = 100,target = 80,interval = 10 },
        };
    }

    static UIManager UIM;
    static CustomerCtr CustomerCtr;

    //程序启动 数据初始化
    public static void StartUp()
    {
        // 需求图+关卡等级+关卡时间+目标过关收益+顾客间隔
        DataInit();
        UIM = GameObject.FindObjectOfType<UIManager>();
        // 通过UI管理模块初始化所有UI组件
        UIM.InitSet();
        CustomerCtr = GameObject.FindObjectOfType<CustomerCtr>();
    }

    public static void Update()
    {
        LevelTimeCountDown();
    }

    // 关卡倒计时
    private static void LevelTimeCountDown()
    {
        if(!levelRunning) return;
        currentLevelInfor.time -= Time.deltaTime;
        UIM.LevelInforUI.RefreshLevelTime((int)currentLevelInfor.time); // 刷新屏幕中显示的剩余时间
        if (currentLevelInfor.time < 0)
        {
            CheckPerformance(); // 本关卡结束时，检查关卡状态
            int count = CustomerCtr.customerQueue.Count;
            for (int i = 0;i < count;i++)
            {
                CustomerCtr.customerQueue[i].Leave();
            }
        }
    }

    private static void CheckPerformance()
    {
        CustomerCtr.RemoveFirstsCustome();
        UIM.DialogUI.gameObject.SetActive(false); // 关卡结束，顾客不再与用户对话
        UIM.DemandDisplayUI.OnHide(); // 关卡结束，没有需求了

        levelRunning = false;
        // 达到了过关收益
        if (incomeCount >= currentLevelInfor.target)
        {
            if (currentLevelInfor.level >= allLevelRequire.Length)
            {
                // 通关所有关卡
                // 清空顾客队列
                GameReset();
                UIM.LevelInforUI.gameObject.SetActive(false);
                // 开启成功组件
                UIM.GameOverUI.gameObject.SetActive(true);
                UIM.GameOverUI.SetTipStr("已完成所有关卡");
            }
            else
            {
                // 开启过关组件
                UIM.LevelSucessUI.gameObject.SetActive(true);
            }
        }
        else // 没过关
        {
            GameReset();
            UIM.LevelInforUI.gameObject.SetActive(false);
            UIM.GameOverUI.gameObject.SetActive(true);
            UIM.GameOverUI.SetTipStr("游戏失败");
        }
        
    }

    //下一关   界面按钮触发
    public static void NextLevel()
    {
        CustomerCtr.QueueMove();
        UIM.LevelSucessUI.gameObject.SetActive(false);
        currentLevelInfor = allLevelRequire[currentLevelInfor.level];
        CustomerCtr.SetCustomerInterval(currentLevelInfor.interval);
        UIM.LevelInforUI.RefreshLevelTaget(currentLevelInfor.target);
        UIM.LevelInforUI.RefreshLevelTitle(currentLevelInfor.level);
        levelRunning = true;
    }

    //游戏开始 界面按钮触发
    public static void StartGame()
    {
        UIM.GameMainUI.gameObject.SetActive(false); // 关主界面UI
        UIM.LevelInforUI.gameObject.SetActive(true); // 开当前关卡信息UI
        currentLevelInfor = allLevelRequire[0];
        incomeCount = 0; // 初始化当前收益为0
        UIM.LevelInforUI.RefreshCurrentIncome(incomeCount); // 刷新一下分数显示组件
        UIM.LevelInforUI.RefreshLevelTaget(currentLevelInfor.target); // 刷新过关收益为当前关卡过关收益
        UIM.LevelInforUI.RefreshLevelTitle(currentLevelInfor.level); // 刷新关卡级别为当前关卡级别
        levelRunning = true;
        CustomerCtr.SetCustomerInterval(currentLevelInfor.interval); // 设置顾客到来间隔时间
        CustomerCtr.StartAddCustomer(); // 开始产生顾客

    }

    /// <summary>
    ///正在等待商品的顾客
    /// </summary>
    static Customer currentCus;
    public static Customer CurrentCus { get { return currentCus; } }

    /// <summary>
    /// 顾客到达指定位置    后生成对应需求 倒计时等待商品中
    /// </summary>
    /// <param name="cus"></param>
    public static void CustomerVisit(Customer cus)
    {
        currentCus = cus;

        List<DemandNode>[] products = DemandControl.CreateDemandProduct();
        // 商品越多、越难做，留给用户制作的时间越多；反之越少
        int countDown = 0;
        for (int i = 0; i < products.Length; i++)
        {
            for (int j = 0; j < products[i].Count; j++)
            {
                countDown += products[i][j].ProductTime;
            }
        }
        // 展示顾客的需求
        UIM.DialogUI.OnShow(countDown);
        UIM.DialogUI.RefreshStr("您好!我想要这些吃的(喝的)。");
        UIM.DemandDisplayUI.OnShow(products);
    }

    /// <summary>
    /// 顾客等待事件超时
    /// </summary>
    public static void CustomerVisitTimeOut()
    {
        CustomerCtr.RemoveFirstsCustome(); // 走掉
        CustomerCtr.QueueMove(); // 整个队伍移动

        // 隐藏对话
        UIM.DialogUI.gameObject.SetActive(false);
        UIM.DemandDisplayUI.OnHide();
    }

    /// <summary>
    /// 商品制作完成，提交当前商品，获得收益,仅当所有商品均符合顾客需求才会调用
    /// </summary>
    public static void DeliveryOfGoods()
    {
        CustomerCtr.RemoveFirstsCustome(); // 第一个顾客走掉
        CustomerCtr.QueueMove(); // 队列移动

        // 关UI
        UIM.DialogUI.gameObject.SetActive(false);
        UIM.DemandDisplayUI.OnHide();
        // 加收益
        incomeCount += 40;
        // 重新渲染收益
        UIM.LevelInforUI.RefreshCurrentIncome(incomeCount);
    }

    /// <summary>
    /// 游戏重置    清空剩余顾客
    /// </summary>
    public static void GameReset()
    {
        CustomerCtr.GameReset();
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public static void RestartGame()
    {
        UIM.GameOverUI.gameObject.SetActive(false);
        StartGame();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


internal struct LevelRequireInfor
{
    internal int level; // �ڼ���
    internal float time; // ��ص���ʱ�೤ʱ��
    internal int target; // Ŀ���������
    internal float interval; // �����˿ͼ��
}

public class GameControl
{

    //���йؿ���Ϣ
    internal static LevelRequireInfor[] allLevelRequire;
    private GameControl() { }

    //��ǰ���йؿ���Ϣ
    private static LevelRequireInfor currentLevelInfor;
    private static int incomeCount;
    private static bool levelRunning;
    public int baseTime = 50;

    public static void DataInit()
    {
        //��������ͼ
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

    //�������� ���ݳ�ʼ��
    public static void StartUp()
    {
        // ����ͼ+�ؿ��ȼ�+�ؿ�ʱ��+Ŀ���������+�˿ͼ��
        DataInit();
        UIM = GameObject.FindObjectOfType<UIManager>();
        // ͨ��UI����ģ���ʼ������UI���
        UIM.InitSet();
        CustomerCtr = GameObject.FindObjectOfType<CustomerCtr>();
    }

    public static void Update()
    {
        LevelTimeCountDown();
    }

    // �ؿ�����ʱ
    private static void LevelTimeCountDown()
    {
        if(!levelRunning) return;
        currentLevelInfor.time -= Time.deltaTime;
        UIM.LevelInforUI.RefreshLevelTime((int)currentLevelInfor.time); // ˢ����Ļ����ʾ��ʣ��ʱ��
        if (currentLevelInfor.time < 0)
        {
            CheckPerformance(); // ���ؿ�����ʱ�����ؿ�״̬
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
        UIM.DialogUI.gameObject.SetActive(false); // �ؿ��������˿Ͳ������û��Ի�
        UIM.DemandDisplayUI.OnHide(); // �ؿ�������û��������

        levelRunning = false;
        // �ﵽ�˹�������
        if (incomeCount >= currentLevelInfor.target)
        {
            if (currentLevelInfor.level >= allLevelRequire.Length)
            {
                // ͨ�����йؿ�
                // ��չ˿Ͷ���
                GameReset();
                UIM.LevelInforUI.gameObject.SetActive(false);
                // �����ɹ����
                UIM.GameOverUI.gameObject.SetActive(true);
                UIM.GameOverUI.SetTipStr("��������йؿ�");
            }
            else
            {
                // �����������
                UIM.LevelSucessUI.gameObject.SetActive(true);
            }
        }
        else // û����
        {
            GameReset();
            UIM.LevelInforUI.gameObject.SetActive(false);
            UIM.GameOverUI.gameObject.SetActive(true);
            UIM.GameOverUI.SetTipStr("��Ϸʧ��");
        }
        
    }

    //��һ��   ���水ť����
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

    //��Ϸ��ʼ ���水ť����
    public static void StartGame()
    {
        UIM.GameMainUI.gameObject.SetActive(false); // ��������UI
        UIM.LevelInforUI.gameObject.SetActive(true); // ����ǰ�ؿ���ϢUI
        currentLevelInfor = allLevelRequire[0];
        incomeCount = 0; // ��ʼ����ǰ����Ϊ0
        UIM.LevelInforUI.RefreshCurrentIncome(incomeCount); // ˢ��һ�·�����ʾ���
        UIM.LevelInforUI.RefreshLevelTaget(currentLevelInfor.target); // ˢ�¹�������Ϊ��ǰ�ؿ���������
        UIM.LevelInforUI.RefreshLevelTitle(currentLevelInfor.level); // ˢ�¹ؿ�����Ϊ��ǰ�ؿ�����
        levelRunning = true;
        CustomerCtr.SetCustomerInterval(currentLevelInfor.interval); // ���ù˿͵������ʱ��
        CustomerCtr.StartAddCustomer(); // ��ʼ�����˿�

    }

    /// <summary>
    ///���ڵȴ���Ʒ�Ĺ˿�
    /// </summary>
    static Customer currentCus;
    public static Customer CurrentCus { get { return currentCus; } }

    /// <summary>
    /// �˿͵���ָ��λ��    �����ɶ�Ӧ���� ����ʱ�ȴ���Ʒ��
    /// </summary>
    /// <param name="cus"></param>
    public static void CustomerVisit(Customer cus)
    {
        currentCus = cus;

        List<DemandNode>[] products = DemandControl.CreateDemandProduct();
        // ��ƷԽ�ࡢԽ�����������û�������ʱ��Խ�ࣻ��֮Խ��
        int countDown = 0;
        for (int i = 0; i < products.Length; i++)
        {
            for (int j = 0; j < products[i].Count; j++)
            {
                countDown += products[i][j].ProductTime;
            }
        }
        // չʾ�˿͵�����
        UIM.DialogUI.OnShow(countDown);
        UIM.DialogUI.RefreshStr("����!����Ҫ��Щ�Ե�(�ȵ�)��");
        UIM.DemandDisplayUI.OnShow(products);
    }

    /// <summary>
    /// �˿͵ȴ��¼���ʱ
    /// </summary>
    public static void CustomerVisitTimeOut()
    {
        CustomerCtr.RemoveFirstsCustome(); // �ߵ�
        CustomerCtr.QueueMove(); // ���������ƶ�

        // ���ضԻ�
        UIM.DialogUI.gameObject.SetActive(false);
        UIM.DemandDisplayUI.OnHide();
    }

    /// <summary>
    /// ��Ʒ������ɣ��ύ��ǰ��Ʒ���������,����������Ʒ�����Ϲ˿�����Ż����
    /// </summary>
    public static void DeliveryOfGoods()
    {
        CustomerCtr.RemoveFirstsCustome(); // ��һ���˿��ߵ�
        CustomerCtr.QueueMove(); // �����ƶ�

        // ��UI
        UIM.DialogUI.gameObject.SetActive(false);
        UIM.DemandDisplayUI.OnHide();
        // ������
        incomeCount += 40;
        // ������Ⱦ����
        UIM.LevelInforUI.RefreshCurrentIncome(incomeCount);
    }

    /// <summary>
    /// ��Ϸ����    ���ʣ��˿�
    /// </summary>
    public static void GameReset()
    {
        CustomerCtr.GameReset();
    }

    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    public static void RestartGame()
    {
        UIM.GameOverUI.gameObject.SetActive(false);
        StartGame();
    }
}

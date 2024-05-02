using System.Collections.Generic;
using UnityEngine;

// 管理顾客的类
public class CustomerCtr : MonoBehaviour
{
    GameObject[] customerPrefabs;

    List<Customer> customerQueue;

    public Transform[] movePath;
    public Transform[] excessivePath;
    public Transform[] moreExcessive;
    public Transform queueOrigin;
    public Transform[] waitPositions;
    private List<Transform> waitList;
    float pauseTime;
    bool running;
    float timeCount;
    float customerInterval;

    private void Start()
    {
        customerPrefabs = new GameObject[transform.childCount]; // 所有顾客的引用
        // 获取引用
        for (int i = 0; i < transform.childCount; i++)
            customerPrefabs[i] = transform.GetChild(i).gameObject;


        customerQueue = new List<Customer>();
        waitList = new List<Transform>();
    }

    private void Update()
    {
        if (running)
        {
            timeCount += Time.deltaTime;
            // 当累计的时间超过了生成顾客的既定间隔时间，添加一个顾客到队列中
            if(timeCount > customerInterval)
            {
                timeCount = 0;
                AddCustomerToQueue();
            }
            // >10个不要再添加顾客
            if (customerQueue.Count > 10)
            {
                running = false;
                pauseTime = Time.time;
            }
        }
    }

    void OnArrivdVisitEndPos(Customer cus)
    {
        GameControl.CustomerVisit(cus); // 顾客开始购买，生成需求
        cus.EndEvent -= OnArrivdVisitEndPos;
    }

    void OnArrivdWaitEndPos(Customer cus)
    {
        cus.QueueWaiting(); // 在队列里站着
        cus.EndEvent -= OnArrivdWaitEndPos;
    }

    void AddCustomerToQueue()
    {
        int p = UnityEngine.Random.Range(0, 100);
        GameObject prefab = customerPrefabs[p / 20]; // 随机挑选一种顾客预制体（总共5种顾客预制体，100内的随机数，100/5=20)
        GameObject obj = Instantiate(prefab); // 创建一个这个预制体的副本
        obj.transform.position = transform.position; 
        obj.SetActive(true); // 激活他
        Customer cus = obj.GetComponent<Customer>();

        if (customerQueue.Count < 1){
            // 就这一个刚生成的顾客，直接让他走到进行购物的区域
            cus.EndEvent += OnArrivdVisitEndPos;
            // 把movePath的实体路径传给他，当作行走路线
            cus.SetPath(movePath);
        }else {
            // 前面有顾客了，给当前顾客添加等待回调，等前面一个人往前走了(或者是结束交易走掉了)再往前走
            cus.EndEvent += OnArrivdWaitEndPos;
            Transform[] waitPath = new Transform[movePath.Length];
            if (customerQueue.Count > 5)
            {
                waitPath = new Transform[excessivePath.Length];
            }
            for (int i = 0; i < waitPath.Length; i++)
            {
                waitPath[i] = movePath[i];
            }
            waitList.Add(waitPositions[waitList.Count]);
            waitPath[waitPath.Length - 1] = waitList[waitList.Count - 1];
            cus.SetPath(waitPath);
        }
  
        customerQueue.Add(cus);
        cus.QueueIndex = customerQueue.Count - 1;

    }

    public void StartAddCustomer()
    {
        AddCustomerToQueue();
        running = true;              
    }

    public void RemoveFirstsCustome()
    {
        customerQueue[0].Leave();
        customerQueue.RemoveAt(0);

    }

    // 顾客队列整体前移
    public void QueueMove()
    {
        waitList.RemoveAt(waitList.Count - 1);
        customerQueue[0].ClearnEvent();

        customerQueue[0].EndEvent += OnArrivdVisitEndPos;
        customerQueue[0].QueuePositionChange(movePath[movePath.Length - 1]);
        for (int i = 1; i < customerQueue.Count; i++)
        {
            customerQueue[i].ClearnEvent();
            customerQueue[i].EndEvent += OnArrivdWaitEndPos;
            customerQueue[i].QueuePositionChange(waitList[i - 1]);
        }
        // >=10个顾客，就先别生成了
        if (!running && customerQueue.Count < 10)
        {
            // <10个，并且停止生成的时间超过了生成间隔，则继续生成
            if (Time.time - pauseTime > customerInterval)
            {
                running = true;
                timeCount = 0;
                AddCustomerToQueue();
            }
        }
    }

    // 重置/清空顾客队列
    public void GameReset()
    {
        running = false;
        for (int i = 0; i < customerQueue.Count; i++)
        {
            Destroy(customerQueue[i].gameObject);
        }
        customerQueue.Clear();
        waitList.Clear();
    }

    // 修改顾客来到的时间间隔
    public void SetCustomerInterval(float time)
    {
        customerInterval = time;
    }

}

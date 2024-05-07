using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Transform leaveEnd;
    [SerializeField]
    public int QueueIndex { get; set; }
    public Transform visitPos;
    private float moveSpeed;
    bool moveState;
    int nextMoveTargetIndex;
    private Transform[] path;
    public event Action<Customer> EndEvent;
    private Animator anim;

    Coroutine moveCoro;
    private Animator Anima { get { if (!anim) anim = GetComponentInChildren<Animator>();  return anim; } }


    void Start()
    {
        moveSpeed = 1f;
    }


    private IEnumerator MovingTarget(Transform pos)
    {
        moveState = true;
        Anima.Play("wak"); // 播放行走动画
        Vector3 dir = pos.position - transform.position;
        transform.GetChild(0).forward = dir;
        float dist = Vector3.Distance(transform.position, pos.position); // 与目标点距离（直线距离）
        float moveDist = 0; // 已经移动距离
        while ( moveDist < dist)
        {
            float offset = Time.deltaTime * moveSpeed; // 本帧走过的距离
            moveDist += offset; 
            transform.position += dir.normalized * offset; // 方向单位向量加上走过的距离
            yield return null; // 挂起协程
        }
        // 抵达目的地（修正位置）
        transform.position = pos.position;
    }


    private IEnumerator Moving()
    {
        for (int i = 0; i < path.Length; i++)
        {
            nextMoveTargetIndex = i;
            yield return MovingTarget(path[i]); // 移动到path[i]的位置
            if ((path[i] == leaveEnd))
                Destroy(gameObject); // 如果顾客离开商摊，销毁对象
        }
        moveState = false;
        EndEvent?.Invoke(this);
        transform.GetChild(0).forward = Vector3.back;
        if (visitPos != path[path.Length - 1]) yield break; // 访问的位置不是路径最后的位置，退出
        Anima.Play("talk1");
        float angle = 0; // 顾客走到目的地转身，和用户（店主）说话
        while(angle < 90)
        {
            float offset = 180 * Time.deltaTime;
            transform.Rotate(Vector3.up,-offset);
            angle += offset;
            yield return null;
        }
        transform.GetChild(0).forward = Vector3.right;
        Invoke("CutIdleState", 5); // 顾客话说完了干站着
    }


    private void CutIdleState()
    {
        if(!moveState)
            Anima.Play("idle");
    }


    private void StartMove()
    {
        moveCoro = StartCoroutine(Moving()); // 将Moving函数作为协程
    }

    // 设置路径，开启移动协程
    public void SetPath(Transform[] targetPath)
    {
        path = targetPath;
        StartMove();
    }

    public void QueueWaiting()
    {
        Anima.Play("idle"); // 用户在队列中等待时播放空闲动画
    }


    public bool IsArrivdWaitFirst(Transform firstPos)
    {
        return path[path.Length - 1] == firstPos;
    }


    public void QueuePositionChange(Transform endPos)
    {
        // 行走状态下的更新
        if(moveState)
        {
            path[path.Length - 1] = endPos; // 更换目标位置设置为终点
            Transform[] newPath = new Transform[path.Length - nextMoveTargetIndex];
            for (int i = 0; i < newPath.Length; i++)
                newPath[i] = path[nextMoveTargetIndex + i]; // 重置数组，把之前走过的位置可以去掉了
            StopCoroutine(moveCoro); // 先销毁之前的协程(原定是走到之前的终点)
            SetPath(newPath); // 用现在的新路径重开一个协程
        }
        else
        {
            nextMoveTargetIndex=0; // 重置下一个目标（因为要开新路径了）,if里面不重置是因为协程会重置。
            SetPath(new Transform[] {endPos}); // 走向给定的终点
        }

    }

    // 清除绑定在顾客身上的事件
    public void ClearnEvent()
    {
        EndEvent = null;
    }


    public void Leave()
    {
        StopCoroutine(moveCoro); // 停止协程
        SetPath(new Transform[] {leaveEnd}); // 让顾客往离开的位置走去
    }
}

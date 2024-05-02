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
        Anima.Play("wak");
        Vector3 dir = pos.position - transform.position;
        transform.GetChild(0).forward = dir;
        float dist = Vector3.Distance(transform.position, pos.position);
        float moveDist = 0;
        while ( moveDist < dist)
        {
            float offset = Time.deltaTime * moveSpeed;
            moveDist += offset;
            transform.position += dir.normalized * offset;
            yield return null;
        }
        transform.position = pos.position;
    }
    private IEnumerator Moving()
    {
        for (int i = 0; i < path.Length; i++)
        {
            nextMoveTargetIndex = i;
            yield return MovingTarget(path[i]);
            if ((path[i] == leaveEnd))
                Destroy(gameObject);
        }
        moveState = false;
        EndEvent?.Invoke(this);
        transform.GetChild(0).forward = Vector3.back;
        if (visitPos != path[path.Length - 1]) yield break;
        Anima.Play("talk1");
        float angle = 0;
        while(angle < 90)
        {
            float offset = 180 * Time.deltaTime;
            transform.Rotate(Vector3.up,-offset);
            angle += offset;
            yield return null;
        }
        transform.GetChild(0).forward = Vector3.right;
        Invoke("CutIdleState", 5);
    }
    private void CutIdleState()
    {
        if(!moveState)
            Anima.Play("idle");
    }
    private void StartMove()
    {
        moveCoro = StartCoroutine(Moving());
    }
    public void SetPath(Transform[] targetPath)
    {
        path = targetPath;
        StartMove();
    }

    public void QueueWaiting()
    {
        Anima.Play("idle");
    }
    public bool IsArrivdWaitFirst(Transform firstPos)
    {
        return path[path.Length - 1] == firstPos;
    }
    public void QueuePositionChange(Transform endPos)
    {
        if(moveState)
        {
            path[path.Length - 1] = endPos;
            Transform[] newPath = new Transform[path.Length - nextMoveTargetIndex];
            for (int i = 0; i < newPath.Length; i++)
                newPath[i] = path[nextMoveTargetIndex + i];
            StopCoroutine(moveCoro);
            SetPath(newPath);
        }
        else
        {
            nextMoveTargetIndex=0;
            SetPath(new Transform[] {endPos});
        }

    }
    public void ClearnEvent()
    {
        EndEvent = null;
    }
    public void Leave()
    {
        StopCoroutine(moveCoro);
        SetPath(new Transform[] {leaveEnd});
    }
}

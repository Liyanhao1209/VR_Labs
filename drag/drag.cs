using UnityEngine;
using UnityEngine.EventSystems;

public class drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rts;
    private void Start()
    {
        rts = GetComponent<RectTransform>();
        Debug.Log(rts);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 拖拽开始时，什么都不做
        Debug.Log("BeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        // 更新GameObject的位置  
        //rts.anchoredPosition += eventData.delta;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 拖拽结束时，什么都不做  
    }

}
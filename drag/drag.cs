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
        // ��ק��ʼʱ��ʲô������
        Debug.Log("BeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        // ����GameObject��λ��  
        //rts.anchoredPosition += eventData.delta;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ��ק����ʱ��ʲô������  
    }

}
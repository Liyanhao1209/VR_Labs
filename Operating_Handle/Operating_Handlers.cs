using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operating_Handlers : MonoBehaviour
{
    private GameObject[] DragControllers;
    private GameObject[] RotateControllers;
    public GameObject drag1;
    public GameObject drag2;
    public GameObject drag3;
    public GameObject drag4;
    public GameObject rotate1;
    public GameObject rotate2;

    public GameObject Image;
    public GameObject frame;

    private bool[] canDrag;
    private bool[] canRotate;

    void Start()
    {
        DragControllers = new GameObject[4];
        RotateControllers = new GameObject[2];

        canDrag = new bool[4];
        canRotate = new bool[2];


        DragControllers[0] = drag1; DragControllers[1] = drag2; DragControllers[2] = drag3; DragControllers[3] = drag4;
        RotateControllers[0] = rotate1; RotateControllers[1] = rotate2;
        for (int i = 0; i < 4; i++)
        {
            canDrag[i] = false;
        }
        for (int i = 0; i < 2; i++)
        {
            canRotate[i] = false;
        }

    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Drag(i);
        }
        for (int i = 0; i < 2; i++)
        {
            Rotate(i);
        }
        moveImage();
        moveRotateButton();
    }

    private void Drag(int index)
    {
        if (!canDrag[index])
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        // 调整选中按钮
        DragControllers[index].transform.position = mousePos;

        Vector3 x_direction,y_direction;
        Vector3 x_point, y_point;
        int x_index, y_index;
        Debug.Log("cs1坐标:"+ DragControllers[1].transform.position+"cs2坐标:"+DragControllers[2].transform.position);
        if (index == 0)
        {
            x_index = 1;
            y_index = 3;
            x_direction = DragControllers[1].transform.position - DragControllers[2].transform.position;
            y_direction = DragControllers[2].transform.position - DragControllers[3].transform.position;
            x_point = DragControllers[1].transform.position;
            y_point = DragControllers[2].transform.position;
        }
        else if (index == 1)
        {
            x_index = 0;
            y_index = 2;
            x_direction = DragControllers[0].transform.position - DragControllers[3].transform.position;
            y_direction = DragControllers[2].transform.position - DragControllers[3].transform.position;
            x_point = DragControllers[0].transform.position;
            y_point = DragControllers[2].transform.position;
        }
        else if (index == 2)
        {
            x_index = 3;
            y_index = 1;
            x_direction = DragControllers[0].transform.position - DragControllers[3].transform.position;
            y_direction = DragControllers[1].transform.position - DragControllers[0].transform.position;
            x_point = DragControllers[0].transform.position;
            y_point = DragControllers[1].transform.position;
        }
        else
        {
            x_index = 2;
            y_index = 0;
            x_direction = DragControllers[1].transform.position - DragControllers[2].transform.position;
            y_direction = DragControllers[0].transform.position - DragControllers[2].transform.position;
            x_point = DragControllers[1].transform.position;
            y_point = DragControllers[0].transform.position;
        }

        x_direction = x_direction.normalized;
        y_direction = y_direction.normalized;
        float x_k = calProjection(x_direction, mousePos,x_point);
        float y_k = calProjection(y_direction, mousePos,y_point);

        DragControllers[x_index].transform.position = x_k * x_direction + x_point;
        DragControllers[y_index].transform.position = y_k * y_direction + y_point;

    }

    private float calProjection(Vector3 direction,Vector3 dp,Vector3 point)
    {
        float a = direction.x;
        float b = direction.y;
        float c = direction.z;

        float x = point.x;
        float y = point.y;
        float z = point.z;

        float p = dp.x;
        float q = dp.y;
        float r = dp.z;

        return p * a + q * b + r * c - a*x - b*y - c*z;
    }

    // 控制图片旋转
    private void Rotate(int index)
    {
        if (Input.GetMouseButton(0) && canRotate[index])
        {
            Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            Image.transform.Rotate(Vector3.forward * mouseDelta.x * 100 * Time.deltaTime, Space.World);
            UpdateDragButton(DragControllers, Image, mouseDelta.x);
        }
    }

    public void UpdateDragButton(GameObject[] DragControllers, GameObject Image, float mouseDelta)
    {
        if (DragControllers == null || Image == null)
        {
            Debug.LogError("DragControllers或Image未设置！");
            return;
        }

        // 获取Image的Transform组件  
        Transform imageTransform = Image.transform;

        // 获取Image的中心点位置  
        Vector3 centerPoint = imageTransform.position;

        // 遍历DragControllers数组中的每个GameObject  
        foreach (GameObject dragController in DragControllers)
        {
            if (dragController != null)
            {
                // 获取当前GameObject的Transform组件  
                Transform dragTransform = dragController.transform;

                // 计算当前GameObject相对于Image中心点的位置向量  
                Vector3 relativePosition = dragTransform.position - centerPoint;

                // 绕Z轴旋转角度  
                float rotationAngle = mouseDelta * 100f * Time.deltaTime; // 你可以根据需要调整这个角度  
                Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

                // 应用旋转到位置向量  
                relativePosition = rotation * relativePosition;

                // 更新GameObject的位置  
                dragTransform.position = centerPoint + relativePosition;
            }
        }
    }


    // 控制图片缩放
    public void moveImage()
    {
        bool flag = false;
        for (int i = 0; i < 4; i++)
        {
            flag |= canDrag[i];
        }
        if (!flag)
        {
            return;
        }
        // 获取四角位置
        Vector3 cs1 = DragControllers[0].transform.position;
        Vector3 cs2 = DragControllers[1].transform.position;
        Vector3 cs3 = DragControllers[2].transform.position;
        Vector3 cs4 = DragControllers[3].transform.position;

        Image.transform.position = (cs3+cs1)/2;

        // 重设图片大小（宽高）
        float height = (cs1 - cs4).magnitude;
        float width = (cs1 - cs2).magnitude;
        Image.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(width, height);
    }

    // 设置宽
    void SetImageWidth(Image image, float width)
    {
        RectTransform rectTransform = image.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    // 设置高
    void SetImageHeight(Image image, float height)
    {
        RectTransform rectTransform = image.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    // 控制旋转按钮移动
    public void moveRotateButton()
    {
        Vector3 cs1 = DragControllers[0].transform.position;
        Vector3 cs2 = DragControllers[1].transform.position;
        Vector3 cs3 = DragControllers[2].transform.position;
        Vector3 cs4 = DragControllers[3].transform.position;

        Vector3 pos1 = Image.transform.position;
        Vector3 pos2 = Image.transform.position;

        pos1.x = (cs1.x + cs2.x) / 2;
        pos2.x = (cs3.x + cs4.x) / 2;
        pos1.y = (cs1.y + cs2.y) / 2;
        pos2.y = (cs3.y + cs4.y) / 2;

        RotateControllers[0].transform.position = pos1;
        RotateControllers[1].transform.position = pos2;
    }

    public void ChangeDragState(int i)
    {
        canDrag[i] = !canDrag[i];
    }

    public void ChangeRotateState(int i)
    {
        canRotate[i] = !canRotate[i];
    }
}

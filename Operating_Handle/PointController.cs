using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
    public GameObject controller_s_1;
    public GameObject controller_s_2;
    public GameObject controller_s_3;
    public GameObject controller_s_4;
    public GameObject controller_r;

    public GameObject Image;
    public GameObject frame;

    public bool movecs1 = false;
    public bool movecs2 = false;
    public bool movecs3 = false;
    public bool movecs4 = false;
    public bool movecr = false;

    void Start()
    {
        
    }

    void Update()
    {
        MoveControllerS1();
        MoveControllerS2();
        MoveControllerS3();
        MoveControllerS4();
        Rotate();
        moveImage();
        moveControllerRotate();
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(0) && movecr)
        {
            Debug.Log("rotate");
            Vector3 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            Image.transform.Rotate(Vector3.forward * mouseDelta.x * 100 * Time.deltaTime, Space.World);
        }
    }

    public void moveImage()
    {
        Vector3 cs1 = controller_s_1.transform.position;
        Vector3 cs2 = controller_s_2.transform.position;
        Vector3 cs3 = controller_s_3.transform.position;
        Vector3 cs4 = controller_s_4.transform.position;

        Vector3 newPos = Image.transform.position;

        newPos.x = (cs1.x + cs2.x) / 2;
        newPos.y = (cs1.y + cs4.y) / 2;

        Image.transform.position = newPos;

        SetImageWidth(Image.GetComponent<Image>(), cs1.x - cs2.x);
        SetImageHeight(Image.GetComponent<Image>(), cs1.y - cs4.y);
    }

    void SetImageWidth(Image image, float width)
    {
        RectTransform rectTransform = image.rectTransform;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    void SetImageHeight(Image image, float height)
    {
        RectTransform rectTransform = image.rectTransform;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    public void moveControllerRotate()
    {
        Vector3 cs1 = controller_s_1.transform.position;
        Vector3 cs2 = controller_s_2.transform.position;
        Vector3 cs3 = controller_s_3.transform.position;
        Vector3 cs4 = controller_s_4.transform.position;

        Vector3 newPos = Image.transform.position;

        newPos.x = (cs1.x + cs2.x) / 2;
        newPos.y = cs1.y + 20;

        controller_r.transform.position = newPos;
    }

    public void MoveControllerS1()
    {
        if (!movecs1) 
            return;
        Vector3 mousePos = Input.mousePosition;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        controller_s_1.transform.position = mousePos;

        Vector3 cs2 = controller_s_2.transform.position;

        cs2.y = mousePos.y;

        controller_s_2.transform.position = cs2;

        Vector3 cs4 = controller_s_4.transform.position;

        cs4.x = mousePos.x;

        controller_s_4.transform.position = cs4;
    }

    public void MoveControllerS2()
    {
        if (!movecs2)
            return;
        Vector3 mousePos = Input.mousePosition;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        controller_s_2.transform.position = mousePos;

        Vector3 cs1 = controller_s_1.transform.position;

        cs1.y = mousePos.y;

        controller_s_1.transform.position = cs1;

        Vector3 cs3 = controller_s_3.transform.position;

        cs3.x = mousePos.x;

        controller_s_3.transform.position = cs3;
    }

    public void MoveControllerS3()
    {
        if (!movecs3)
            return;
        Vector3 mousePos = Input.mousePosition;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        controller_s_3.transform.position = mousePos;

        Vector3 cs4 = controller_s_4.transform.position;

        cs4.y = mousePos.y;

        controller_s_4.transform.position = cs4;

        Vector3 cs2 = controller_s_2.transform.position;

        cs2.x = mousePos.x;

        controller_s_2.transform.position = cs2;
    }

    public void MoveControllerS4()
    {
        if (!movecs4)
            return;
        Vector3 mousePos = Input.mousePosition;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));

        controller_s_4.transform.position = mousePos;

        Vector3 cs3 = controller_s_3.transform.position;

        cs3.y = mousePos.y;

        controller_s_3.transform.position = cs3;

        Vector3 cs1 = controller_s_1.transform.position;

        cs1.x = mousePos.x;

        controller_s_1.transform.position = cs1;
    }
    public void CanMoveCS1()
    {
        movecs1 = !movecs1;
    }

    public void CanMoveCS2()
    {
        movecs2 = !movecs2;
    }

    public void CanMoveCS3()
    {
        movecs3 = !movecs3;
    }

    public void CanMoveCS4()
    {
        movecs4 = !movecs4;
    }

    public void CanRotate()
    {
        movecr = !movecr;
        //frame.rect = 90;
    }
}

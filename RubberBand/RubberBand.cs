using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    private Vector3 start;
    private bool startReady = false;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        ConfigLine();
    }

    void ConfigLine()
    {
        GameObject lineObject = new GameObject("Line");
        line = lineObject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.yellow;
        line.endColor = Color.green;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startReady && Input.GetMouseButtonDown(0))
        {
            startReady = true;
            start = GetMouseWorldPosition();
        }

        if (startReady)
        {
            Vector3 curr = GetMouseWorldPosition();
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, curr);
            if (Input.GetMouseButtonDown(0))
            {
                ConfigLine();
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}

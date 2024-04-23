using UnityEngine;

public class Circle_Drawer : MonoBehaviour
{
    private LineRenderer circle;
    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<LineRenderer>();
        circle.material = new Material(Shader.Find("Sprites/Default"));
        circle.startColor = Color.blue;
        circle.endColor = Color.red;
        circle.startWidth = 1f;
        circle.endWidth = 1f;

        Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
        float radius = 20.0f;
        Vector3 vec = new Vector3(radius, 0.0f, 0.0f);
        float rotAngle = 0.0f;
        while (rotAngle < 360.0f)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);
            Vector3 tmp = rotation * vec;
            Vector3 dp = center + tmp;
            circle.positionCount++;
            circle.SetPosition(circle.positionCount - 1, dp);
            rotAngle += 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

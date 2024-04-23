using UnityEngine;

public class Gravitational : MonoBehaviour
{
    struct GravityArea
    {
        public Vector3 start;
        public Vector3 end;
        public float radius;
    }
    private LineRenderer gravity;
    private LineRenderer lineRenderer;
    private GravityArea area;
    bool isDrawing = false;

    void Start()
    {
        area = new GravityArea();
        area.start = new Vector3(0.0f, 0.0f, 0.0f);
        area.end = new Vector3(50.0f, 50.0f, 0.0f);
        area.radius = 10.0f;

        ConfigGravity();
        ConfigLineRenderer();
    }

    void ConfigGravity()
    {
        gravity = GetComponent<LineRenderer>();

        gravity.positionCount = 2;
        gravity.material = new Material(Shader.Find("Sprites/Default"));
        gravity.startColor = Color.blue;
        gravity.endColor = Color.red;
        gravity.startWidth = 1f;
        gravity.endWidth = 1f;
        gravity.SetPosition(0, area.start);
        gravity.SetPosition(1, area.end);

    }

    void ConfigLineRenderer()
    {
        GameObject lineObject = new GameObject("Line");
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;

    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            ConfigLineRenderer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
        }

        if (isDrawing && Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            Vector3 direction = (area.start - area.end).normalized;
            float k = calProjection(direction, mousePos, area.start);
            Vector3 projection = k * direction + area.start;

            if(projection.x<=area.end.x && projection.x>=area.start.x)
            {
                Vector3 othor = mousePos - projection;
                float dis = othor.magnitude;
                if(dis >= area.radius)
                {
                    LineAddPoint(lineRenderer, mousePos);
                }
                else
                {
                    ConfigLineRenderer();
                }
            }
            else
            {
                if((mousePos-area.start).magnitude>=area.radius && (mousePos - area.end).magnitude >= area.radius)
                {
                    LineAddPoint(lineRenderer, mousePos);
                }
            }
        }

    }

    void LineAddPoint(LineRenderer lr,Vector3 pos)
    {
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, pos);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private float calProjection(Vector3 direction, Vector3 dp, Vector3 point)
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

        return p * a + q * b + r * c - a * x - b * y - c * z;
    }
}

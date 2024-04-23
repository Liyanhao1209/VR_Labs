using UnityEngine;

public class Geo_Constraint : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isDrawing = false;
    private bool isRecording = false;
    private Vector3 mouseBeginPos;
    private Vector3 mouseEndPos;
    private Vector3 mouseDirectPos;
    private int interFrame = 0;
    public GameObject Lines;
    private bool toY = false;

    void ConfigLineRenderer()
    {
        GameObject lineObject = new GameObject("Line");
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 8f;
        lineRenderer.endWidth = 8f;

        lineObject.transform.SetParent(Lines.transform);
    }


    void Start()
    {
        ConfigLineRenderer();
    }

    void Update()
    {
        // 鼠标抬起停止绘画，重新配置下一条线
        if (Input.GetMouseButtonUp(0)){
            isDrawing = false;
            interFrame = 0;
            ConfigLineRenderer();
        }

        // 绘画过程中
        if (isDrawing && Input.GetMouseButton(0)){
            // 等待稳定
            interFrame++;
            Vector3 mousePos = GetMouseWorldPosition();

            if(interFrame>10){
                if(isRecording){
                    // 计算方向
                    mouseDirectPos = mousePos - mouseBeginPos;
                    isRecording = false; // 只需要计算一次即可
                    toY = Mathf.Abs(mouseDirectPos.x) < Mathf.Abs(mouseDirectPos.y); // 是否吸附到y轴上

                    for (int i = 0; i < lineRenderer.positionCount; i++){ 
                        if (toY){
                            mousePos.x = NearestNeighbourPos(mouseBeginPos.x,50.0f); // 近邻网格吸附
                            lineRenderer.SetPosition(i, mousePos);
                        }else{
                            mousePos.y = NearestNeighbourPos(mouseBeginPos.y,50.0f);
                            lineRenderer.SetPosition(i, mousePos);
                        }
                    }
                }

                if(toY){
                    mousePos.x = NearestNeighbourPos(mouseBeginPos.x,50.0f);
                }else{
                    mousePos.y = NearestNeighbourPos(mouseBeginPos.y,50.0f);
                }
            }

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
        }

        if (Input.GetMouseButtonDown(0)){
            mouseBeginPos = GetMouseWorldPosition();
            isDrawing = true;
            isRecording = true;
        }
    }

    float NearestNeighbourPos(float begin,float range){
        float tmp = begin%range;
        if(tmp>=range/2){
            return begin - begin%range+range;
        }
        return begin-begin%range;
    }   


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}


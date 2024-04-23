using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Transform[] objectsToRotate;
    public float rotationSpeed = 100.0f; 
    public Vector3 pivotPoint; 

    private void Start()
    {
        pivotPoint = transform.position;
    }

    void Update()
    {
       /* 
        foreach (var obj in objectsToRotate)
        {
            obj.RotateAround(pivotPoint, Vector3.forward, rotationSpeed * Time.deltaTime);
        }*/
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;
    Camera cam;
   /* public float minZoom = 60f;
    public float maxZoom = 10f;
    Bounds bounds;
    public float zoomLimiter = 50f;
    Vector2 MousePos;
    Vector3 velocity;
    Bounds boundy;*/
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        //smoothSpeed = target.GetComponent<Rigidbody2D>().velocity.magnitude;
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }

    /* void LateUpdate()
     {
         MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
         Move();
         Zoom();
         Vector3 desirdPos = target.position + offset;
         Vector3 smoothedPos = Vector3.Lerp(transform.position, desirdPos, smoothSpeed*Time.deltaTime);
         transform.position = smoothedPos;


     }
     void Move()
     {
         Vector3 centerPoint = GetCenterPoint();
         Vector3 newPosition = centerPoint + offset;
         transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothSpeed);


     }
     void Zoom()
     {
         float newZoom = Mathf.Lerp(maxZoom, minZoom, Mathf.Sqrt(Mathf.Pow(boundy.size.x, 2) + Mathf.Pow(boundy.size.y * 2, 2)) / zoomLimiter);
         print(newZoom);
         cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
     }
     Vector3 GetCenterPoint()
     {
         boundy = new Bounds(target.position, Vector3.zero);
         boundy.Encapsulate(MousePos);
         return boundy.center;

     }*/
}

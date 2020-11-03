using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Origin; // place where mouse is first pressed
    private Vector3 Diference; // change in position of mouse relative to origin
    private Camera cam;
    public float minimumZoom = 4.0f;
    public float maximumZoom = 16.0f;
    private float targetZoom;
    public float zoomFactor = 3f;
    public float zoomLerpSpeed = 10;
 
void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom = targetZoom - scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, minimumZoom, maximumZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime*zoomLerpSpeed);
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Origin = MousePos();
        }
        if (Input.GetMouseButton(1))
        {
            Diference = MousePos() - transform.position;
            transform.position = Origin - Diference;
        }
    }
    // return the position of the mouse in world coordinates (helper method)
    Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

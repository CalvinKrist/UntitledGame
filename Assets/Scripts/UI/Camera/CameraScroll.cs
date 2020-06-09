using System;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float minScreenSize = 1;
    void Update()
    {
        var scrollDistance = Input.GetAxis("Mouse ScrollWheel");
        GetComponent<Camera>().orthographicSize = Math.Max(GetComponent<Camera>().orthographicSize + -1 * scrollSpeed * scrollDistance, minScreenSize);
    }
}

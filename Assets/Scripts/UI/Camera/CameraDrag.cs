using Unity.Entities;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin, cameraOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            cameraOrigin = Camera.main.transform.position;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 translation = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Camera.main.transform.Translate(translation, Space.World);
    }


}

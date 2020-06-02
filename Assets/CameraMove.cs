using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float moveSpeed = 0.25f;
    private float scrollSpeed = 10f;

    void Update()
    {
        if (Input.mousePosition.x > (9*Screen.width / 10) )
        {
            transform.position += moveSpeed * new Vector3(1, 0, 0);
        }

        if (Input.mousePosition.x < ( Screen.width / 10))
        {
            transform.position += moveSpeed * new Vector3(-1, 0, 0);
        }
        if (Input.mousePosition.y > (9 * Screen.height / 10))
        {
            transform.position += moveSpeed * new Vector3(0, 0, 1);
        }
        if (Input.mousePosition.y < ( Screen.height / 10))
        {
            transform.position += moveSpeed * new Vector3(0, 0, -1);
        }

    }
}

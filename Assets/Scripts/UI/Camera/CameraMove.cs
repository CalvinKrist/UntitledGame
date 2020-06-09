using UnityEngine;

public class CameraMove : MonoBehaviour
{
    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    public float moveSpeed = 0.25f;

    void Update()
    {
        if (IsMouseOverGameWindow)
        {
            if (Input.mousePosition.x > (9 * Screen.width / 10))
            {
                transform.position += moveSpeed * Vector3.right;
            }

            if (Input.mousePosition.x < (Screen.width / 10))
            {
                transform.position += moveSpeed * Vector3.left;
            }
            if (Input.mousePosition.y > (9 * Screen.height / 10))
            {
                transform.position += moveSpeed * Vector3.up;
            }
            if (Input.mousePosition.y < (Screen.height / 10))
            {
                transform.position += moveSpeed * Vector3.down;
            }
        }
    }
}

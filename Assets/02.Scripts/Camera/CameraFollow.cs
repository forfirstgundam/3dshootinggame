using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform[] Targets;
    public int ViewPoint = 0;

    private void Update()
    {
        // interpoling, smoothing ±â¹ý
        transform.position = Targets[ViewPoint].position;

        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            ViewPoint = 0;
        } else if(Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            ViewPoint = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            ViewPoint = 2;
        }
    }
}

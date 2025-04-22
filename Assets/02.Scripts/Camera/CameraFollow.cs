using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3[] Targets;
    public Transform Player;
    public int ViewPoint = 0;

    private void Revolve()
    {
        float xdegree = Player.eulerAngles.y;

        float yawRad = xdegree * Mathf.Deg2Rad;

        float x = Mathf.Sin(yawRad) * Targets[ViewPoint].z;
        float z = Mathf.Cos(yawRad) * Targets[ViewPoint].z;

        transform.position = Player.position + new Vector3(x, Targets[ViewPoint].y, z);
        Debug.Log($"camera is here : {transform.position}");
    }

    private void ViewSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            ViewPoint = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            ViewPoint = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            ViewPoint = 2;
        }
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // interpoling, smoothing ±â¹ý
        ViewSwitch();
        Revolve();
    }
}

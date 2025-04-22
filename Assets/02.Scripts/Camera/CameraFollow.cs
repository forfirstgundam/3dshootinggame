using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3[] Targets;
    public Transform Player;
    public int ViewPoint = 1;

    public float RotationSpeed = 100f;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Revolve()
    {
        float xdegree = Player.eulerAngles.y;

        float yawRad = xdegree * Mathf.Deg2Rad;

        float x = Mathf.Sin(yawRad) * Targets[ViewPoint].z;
        float z = Mathf.Cos(yawRad) * Targets[ViewPoint].z;

        transform.position = Player.position + new Vector3(x, Targets[ViewPoint].y, z);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;

        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);
        Vector3 dir = new Vector3(-_rotationY, _rotationX, 0);

        /* 회전 공식 : 
         * 새로운 위치 = 현재 위치 + 속도 * 시간
         * 새로운 각도 = 현재 각도 + 회전 속도 * 시간
         */
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
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
        // interpoling, smoothing 기법
        ViewSwitch();
        Revolve();
        if (ViewPoint != 0)
        {
            transform.LookAt(Player.position);
        }
        else
        {
            Rotate();
            //Player.eulerAngles = new Vector3(0, _rotationX, 0);
        }
    }
}

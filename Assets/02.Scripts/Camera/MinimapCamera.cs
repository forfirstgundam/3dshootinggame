using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Camera _minimapCamera;
    public static MinimapCamera Instance;

    public Transform Target;
    public float YOffset = 10f;
    public float MaxSize = 20f;
    public float MinSize = 5f;

    public float EachClick = 2f;

    private void Awake()
    {
        _minimapCamera = GetComponent<Camera>();
        Instance = this;
    }
    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;

        transform.position = newPosition;

        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }

    public void MinimapScaleChange(bool zoom)
    {
        if (zoom)
        {
            _minimapCamera.orthographicSize -= EachClick;
        }
        else
        {
            _minimapCamera.orthographicSize += EachClick;
        }
        _minimapCamera.orthographicSize = Mathf.Clamp(_minimapCamera.orthographicSize, MinSize, MaxSize);
    }
}

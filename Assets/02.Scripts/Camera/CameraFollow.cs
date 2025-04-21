using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // interpoling, smoothing ±â¹ý
        transform.position = Target.position;
    }
}

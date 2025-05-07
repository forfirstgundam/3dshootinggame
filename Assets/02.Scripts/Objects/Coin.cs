using UnityEngine;

public class Coin : MonoBehaviour
{
    public float SpinSpeed = 20f;
    public float HoverOffset = 0.08f;
    public float RaycastDistance = 5f;

    // spin while existing
    void Update()
    {
        transform.Rotate(Vector3.left, SpinSpeed * Time.deltaTime);

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, RaycastDistance))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + HoverOffset;
            transform.position = pos;
        }
    }
}

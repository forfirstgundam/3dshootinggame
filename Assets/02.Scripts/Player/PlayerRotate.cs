using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 100f;

    private float _rotationX = 0;

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        float mouseX = Input.GetAxis("Mouse X");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}

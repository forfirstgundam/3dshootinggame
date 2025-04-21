using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 100f;

    // 카메라 각도의 기준은 0도부터 시작
    private float _rotationX = 0;
    private float _rotationY = 0;

    /* 카메라 회전 스크립트
     * 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전
     * 
     * 1. 마우스 입력을 받는다
     * 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
     * 3. 카메라를 그 방향으로 회전한다.
     */

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log($"Mouse X : {mouseX}, Mouse Y : {mouseY}");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;

        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);
        Vector3 dir = new Vector3(-_rotationY, _rotationX, 0);

        /* 회전 공식 : 
         * 새로운 위치 = 현재 위치 + 속도 * 시간
         * 새로운 각도 = 현재 각도 + 회전 속도 * 시간
         */
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }
}

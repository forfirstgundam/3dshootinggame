using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 100f;

    // ī�޶� ������ ������ 0������ ����
    private float _rotationX = 0;
    private float _rotationY = 0;

    /* ī�޶� ȸ�� ��ũ��Ʈ
     * ��ǥ : ���콺�� �����ϸ� ī�޶� �� �������� ȸ��
     * 
     * 1. ���콺 �Է��� �޴´�
     * 2. ���콺 �Է����κ��� ȸ����ų ������ �����.
     * 3. ī�޶� �� �������� ȸ���Ѵ�.
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

        /* ȸ�� ���� : 
         * ���ο� ��ġ = ���� ��ġ + �ӵ� * �ð�
         * ���ο� ���� = ���� ���� + ȸ�� �ӵ� * �ð�
         */
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }
}

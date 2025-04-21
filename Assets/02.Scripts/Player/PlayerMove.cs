using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7f;
    /* ��ǥ : wasd�� ������ ĳ���Ͱ� �̵� (ī�޶� ���⿡ �°�)
     * 
     * ���� ���� : 
     * 1. Ű���� �Է�
     * 2. �Է����κ��� ���� ����
     * 3. ���⿡ ���� �÷��̾� �̵�
     */

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        //TransformDirection : ���� ������ ���� -> ���� ������ ����

        transform.position += dir * MoveSpeed * Time.deltaTime;
    }
}

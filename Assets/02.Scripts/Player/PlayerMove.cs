using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpPower = 5f;

    private CharacterController _characterController;
    private const float GRAVITY = -9.8f; // �߷�
    private float _yVelocity = 0f;       // �߷� ���ӵ�

    private bool _isJumping = false;
    /* ��ǥ : wasd�� ������ ĳ���Ͱ� �̵� (ī�޶� ���⿡ �°�)
     * 
     * ���� ���� : 
     * 1. Ű���� �Է�
     * 2. �Է����κ��� ���� ����
     * 3. ���⿡ ���� �÷��̾� �̵�
     */

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        //dir.y = 0;
        //TransformDirection : ���� ������ ���� -> ���� ������ ����

        if (_characterController.isGrounded) _isJumping = false;

        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            _yVelocity = JumpPower;
            _isJumping = true;
        }

        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
}

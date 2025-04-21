using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float RunSpeed = 12f;
    public float JumpPower = 5f;

    public float Stamina = 50f;
    private float _staminaUseSpeed = 10f;
    private float _staminaFillSpeed = 3f;

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
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0f)
        {
            _characterController.Move(dir * RunSpeed * Time.deltaTime);
            Stamina -= _staminaUseSpeed * Time.deltaTime;
        }
        else
        {
            Stamina += _staminaFillSpeed * Time.deltaTime;
            _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        }
        Debug.Log($"current stamina : {Stamina}");
    }
}

using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float RunSpeed = 12f;
    public float RollSpeed = 25f;

    public float JumpPower = 5f;
    public int AvailableJump = 2;

    private int _maxJump = 2;
    private bool _isRolling = false;

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

    IEnumerator Roll(Vector3 dir)
    {
        _isRolling = true;
        float rolltime = 0.1f;
        float curtime = 0f;
        while (curtime <= rolltime)
        {
            _characterController.Move(dir * RollSpeed* Time.deltaTime);
            curtime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("has rolled");
        _isRolling = false;
        yield return null;
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

        if (_characterController.isGrounded)
        {
            AvailableJump = _maxJump;
            _isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && AvailableJump >0)
        {
            _yVelocity = JumpPower;
            _isJumping = true;
            AvailableJump--;
            Debug.Log($"you can jump {AvailableJump}");
        }

        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        //transform.position += dir * MoveSpeed * Time.deltaTime;

        //shift�� ������ �ٱ�
        //�⺻ �̵���: ������ ���� �ƴ� ��
        if (!_isRolling)
        {
            if (Input.GetKey(KeyCode.LeftShift) && PlayerStamina.Stamina > 0f)
            {
                _characterController.Move(dir * RunSpeed * Time.deltaTime);
                PlayerStamina.Stamina -= PlayerStamina.DashUseRate * Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.E) && PlayerStamina.Stamina > PlayerStamina.RollUsage)
            {
                PlayerStamina.Stamina -= PlayerStamina.RollUsage;
                StartCoroutine(Roll(dir));
            }
            else
            {
                PlayerStamina.Stamina += PlayerStamina.FillRate * Time.deltaTime;
                PlayerStamina.Stamina = Mathf.Clamp(PlayerStamina.Stamina, 0, PlayerStamina.MaxStamina);
                _characterController.Move(dir * MoveSpeed * Time.deltaTime);
            }
        }

        
        Debug.Log($"current stamina : {PlayerStamina.Stamina}");
    }
}

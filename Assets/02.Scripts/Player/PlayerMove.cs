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
    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f;       // 중력 가속도

    private bool _isJumping = false;
    /* 목표 : wasd를 누르면 캐릭터가 이동 (카메라 방향에 맞게)
     * 
     * 구현 순서 : 
     * 1. 키보드 입력
     * 2. 입력으로부터 방향 설정
     * 3. 방향에 따라 플레이어 이동
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
        //TransformDirection : 로컬 공간의 벡터 -> 월드 공간의 벡터

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

        //shift를 눌러서 뛰기
        //기본 이동들: 구르기 중이 아닐 때
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

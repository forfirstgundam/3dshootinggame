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

    private bool _climbWall = false;

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(Vector3.Angle(hit.normal, Vector3.up) > 85f)
        {
            _climbWall = true;
        }
    }
    private bool checkWallInFront()
    {
        CollisionFlags collisionplace = _characterController.collisionFlags;
        return (collisionplace & CollisionFlags.Sides) != 0;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Movement(float h, float v, float speed)
    {
        Vector3 dir = new Vector3(h, 0, v).normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = _yVelocity; // 중력 적용
        _characterController.Move(dir * speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            AvailableJump = _maxJump;
            _isJumping = false;
        }

        _yVelocity = JumpPower;
        _isJumping = true;
        AvailableJump--;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        Debug.Log($"{dir}");
        //TransformDirection : 로컬 공간의 벡터 -> 월드 공간의 벡터

        if (!_climbWall)
        {
            _yVelocity += GRAVITY * Time.deltaTime;
        }
        else
        {
            _yVelocity = 0f;
        }

        //transform.position += dir * MoveSpeed * Time.deltaTime;

        //shift를 눌러서 뛰기
        //기본 이동들: 구르기 중이 아닐 때
        if (!_isRolling)
        {
            if (_climbWall)
            {
                if(PlayerStamina.Stamina > 0 && checkWallInFront())
                {
                    Vector3 forward = Camera.main.transform.forward;
                    forward.y = 0;
                    forward = forward.normalized;

                    Vector3 wallRight = Vector3.Cross(Vector3.up, forward);
                    Vector3 climbDir = (Vector3.up * v + wallRight * h).normalized;

                    _characterController.Move(climbDir * MoveSpeed * Time.deltaTime);
                    PlayerStamina.Stamina -= PlayerStamina.ClimbUseRate * Time.deltaTime;
                }
                else
                {
                    _climbWall = false;
                }

            } else if (Input.GetButtonDown("Jump") && AvailableJump > 0)
            {
                Jump();
                Debug.Log($"you can jump {AvailableJump}");
            }
            else if (Input.GetKey(KeyCode.LeftShift) && PlayerStamina.Stamina > 0f)
            {
                Movement(h, v, RunSpeed);
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
                Movement(h, v, MoveSpeed);
            }
        }
        
        Debug.Log($"current stamina : {PlayerStamina.Stamina}");
    }
}

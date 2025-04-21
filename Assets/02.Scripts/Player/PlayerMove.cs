using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float JumpPower = 5f;

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
        //TransformDirection : 로컬 공간의 벡터 -> 월드 공간의 벡터

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

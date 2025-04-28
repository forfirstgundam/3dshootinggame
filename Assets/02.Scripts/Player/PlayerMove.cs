using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerStatsSO Stats;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _runSpeed = 12f;
    [SerializeField] private float _rollSpeed = 25f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _maxJumpCount = 2;

    // Constants
    private const float GRAVITY = -9.8f;
    private const float ROLL_DURATION = 0.3f;
    private const float WALL_ANGLE_THRESHOLD = 85f;

    // Component references
    private CharacterController _characterController;
    private Animator _animator;
    private Camera mainCamera;

    // Movement state
    private float _verticalVelocity;
    private int _availableJumps;
    private bool _isRolling;
    private bool _isClimbing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        _availableJumps = _maxJumpCount;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        HandleMovementInput();
        ApplyGravity();
        HandleStaminaRegeneration();
        if (_characterController.isGrounded)
        {
            _availableJumps = _maxJumpCount;
            _verticalVelocity = 0;
        }
    }

    private void HandleMovementInput()
    {
        if (_isRolling) return;

        Vector3 moveDirection = GetMovementDirection();

        if (_isClimbing)
        {
            HandleClimbing(moveDirection);
        }
        else if (Input.GetButtonDown("Jump") && CanJump())
        {
            PerformJump();
        }
        else if (Input.GetKey(KeyCode.LeftShift) && CanRun())
        {
            PerformRun(moveDirection);
        }
        else if (Input.GetKeyDown(KeyCode.E) && CanRoll())
        {
            PerformRoll(moveDirection);
        }
        else
        {
            PerformNormalMovement(moveDirection);
        }
    }

    private Vector3 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        _animator.SetFloat("MoveAmount", direction.magnitude);

        direction = direction.normalized;

        return mainCamera.transform.TransformDirection(direction);
    }

    private void HandleClimbing(Vector3 moveDirection)
    {
        if (Stats.Stamina > 0 && IsWallInFront())
        {
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            forward = forward.normalized;

            Vector3 wallRight = Vector3.Cross(Vector3.up, forward);
            Vector3 climbDirection = (Vector3.up * Input.GetAxisRaw("Vertical") + 
                                    wallRight * Input.GetAxisRaw("Horizontal")).normalized;

            _characterController.Move(climbDirection * _moveSpeed * Time.deltaTime);
            ConsumeStamina(Stats.ClimbUseRate * Time.deltaTime);
        }
        else
        {
            _isClimbing = false;
        }
    }

    private void PerformNormalMovement(Vector3 direction)
    {
        direction.y = _verticalVelocity;
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
    }

    private void PerformRun(Vector3 direction)
    {
        direction.y = _verticalVelocity;
        _characterController.Move(direction * _runSpeed * Time.deltaTime);
        ConsumeStamina(Stats.DashUseRate * Time.deltaTime);
    }

    private void PerformJump()
    {
        _verticalVelocity = _jumpPower;
        _availableJumps--;
    }

    private IEnumerator Roll(Vector3 direction)
    {
        _isRolling = true;
        float elapsedTime = 0f;
        float initialX = transform.eulerAngles.x;

        while (elapsedTime <= ROLL_DURATION)
        {
            _characterController.Move(direction * _rollSpeed * Time.deltaTime);

            float t = elapsedTime / ROLL_DURATION;
            float xRotation = Mathf.Lerp(initialX, initialX + 359f, t);
            Vector3 currentEuler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(xRotation, currentEuler.y, currentEuler.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(initialX, transform.eulerAngles.y, transform.eulerAngles.z);
        _isRolling = false;
    }

    private void PerformRoll(Vector3 direction)
    {
        ConsumeStamina(Stats.RollUsage);
        StartCoroutine(Roll(direction));
    }

    private void ApplyGravity()
    {
        if (!_isClimbing)
        {
            _verticalVelocity += GRAVITY * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = 0f;
        }
    }

    private void HandleStaminaRegeneration()
    {
        if (!_isRolling && !Input.GetKey(KeyCode.LeftShift) && !_isClimbing)
        {
            Stats.Stamina += Stats.FillRate * Time.deltaTime;
            Stats.Stamina = Mathf.Clamp(Stats.Stamina, 0, Stats.MaxStamina);
        }
    }

    private void ConsumeStamina(float amount)
    {
        Stats.Stamina -= amount;
        Stats.Stamina = Mathf.Max(0, Stats.Stamina);
    }

    private bool CanJump() => _availableJumps > 0;
    private bool CanRun() => Stats.Stamina > 0f;
    private bool CanRoll() => Stats.Stamina > Stats.RollUsage;
    private bool IsWallInFront()
    {
        Vector3 origin = transform.position;
        origin.y = origin.y - 1;
        Vector3 forward = transform.forward;
        float rayDistance = 0.6f;

        return Physics.Raycast(origin, forward, rayDistance);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _isClimbing = Vector3.Angle(hit.normal, Vector3.up) > WALL_ANGLE_THRESHOLD;
    }
}

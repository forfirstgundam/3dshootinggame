using UnityEngine;

public enum EnemyState
{
    Idle,
    Trace,
    Return,
    Attack,
    Hit,
    Die
}

// 인공지능 : 지능을 가지고 행동하는 알고리즘
// 반응형 / 계획형 -> 규칙 기반 인공지능(전통적인 방식) 
//                 -> 제어문 기반

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Idle;
    private CharacterController _characterController;
    private Vector3 _startPosition;

    private GameObject _player;

    public float MoveSpeed = 5f;
    public float FindDistance = 7f;
    public float ReturnDistance = 15f;
    public float AttackDistance = 3f;

    public float AttackCoolTime = 1f;
    private float _attackTimer = 1f;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case (EnemyState.Idle):
                {
                    Idle();
                    break;
                }
            case (EnemyState.Trace):
                {
                    Trace();
                    break;
                }
            case (EnemyState.Return):
                {
                    Return();
                    break;
                }
            case (EnemyState.Attack):
                {
                    Attack();
                    break;
                }
            case (EnemyState.Hit):
                {
                    Hit();
                    break;
                }
            case (EnemyState.Die):
                {
                    Die();
                    break;
                }
        }
    }

    private void Idle()
    {
        // 가만히 있기

        // 가까워질 경우 Trace로 전환 
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {
        // 멀어질 경우 Return으로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // 공격 범위만큼 가까워지면 Attack으로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 플레이어 따라오기
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // 플레이어와 가까워질 경우 Trace로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }

        // 원래 위치일 경우 Idle로 전환
        if (Vector3.Distance(transform.position, _startPosition) <= 0.1f)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 원래 위치로 복귀
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 멀어질 경우 Trace로 전환
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 1f;
            return;
        }

        // 플레이어를 공격
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCoolTime)
        {
            Debug.Log("플레이어를 공격합니다");
            _attackTimer = 0f;
        }

    }

    private void Hit()
    {

    }

    private void Die()
    {

    }
}

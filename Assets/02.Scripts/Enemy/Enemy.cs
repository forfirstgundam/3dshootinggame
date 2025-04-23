using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patrol,
    Trace,
    Return,
    Attack,
    Hit,
    Die,
}

// 인공지능 : 지능을 가지고 행동하는 알고리즘
// 반응형 / 계획형 -> 규칙 기반 인공지능(전통적인 방식) 
//                 -> 제어문 기반

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentState = EnemyState.Idle;
    private CharacterController _characterController;
    private Vector3 _returnPosition;
    public Transform[] PatrolPositions;

    private int _curPos;

    private GameObject _player;

    public float MoveSpeed = 5f;
    public float FindDistance = 7f;
    public float ReturnDistance = 15f;
    public float AttackDistance = 3f;

    public float AttackCoolTime = 1f;
    private float _attackTimer = 1f;

    public float IdleTime = 5f;
    private float _idleTimer = 0f;

    public float HitTime = 0.5f;
    public float DieTime = 1f;

    private Coroutine _beingHit;

    public int Health = 100;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _returnPosition = transform.position;
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
            case (EnemyState.Patrol):
                {
                    Patrol();
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
                    break;
                }
            case (EnemyState.Die):
                {
                    StartCoroutine(Die());
                    break;
                }
        }
    }

    private void Idle()
    {
        // 일정 시간 지나면 Patrol로 전환
        if(_idleTimer >= IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            CurrentState = EnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }

        // 가까워질 경우 Trace로 전환 
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
            _idleTimer = 0f;
            return;
        }

        _idleTimer += Time.deltaTime;
    }

    private void Patrol()
    {
        // 지정 위치로 이동했을 경우 : 다음 위치 지정, 다시 idle로
        if (Vector3.Distance(transform.position, PatrolPositions[_curPos].position) <= 0.1f)
        {
            transform.position = PatrolPositions[_curPos].position;
            _returnPosition = PatrolPositions[_curPos].position;
            Debug.Log("상태전환: Patrol -> Idle");
            CurrentState = EnemyState.Idle;
            if(_curPos >= PatrolPositions.Length - 1)
            {
                _curPos = 0;
            }else
            {
                _curPos++;
            }
            return;
        }

        // 3가지 위치로 이동하기
        Vector3 dir = (PatrolPositions[_curPos].position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
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
        if (Vector3.Distance(transform.position, _returnPosition) <= 0.1f)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _returnPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 원래 위치로 복귀
        Vector3 dir = (_returnPosition - transform.position).normalized;
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

    private IEnumerator Hit(Vector3 dir, float knockback)
    {
        // 일정 시간 경직
        float _timer = 0f;
        while(_timer <= HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // 상태 전환
        Debug.Log("상태전환: Hit -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(DieTime);
        gameObject.SetActive(false);
    }

    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Hit || CurrentState == EnemyState.Die) return;

        Health -= damage.Value;
        if(Health <= 0)
        {
            Debug.Log("상태전환: Hit -> Die");
            CurrentState = EnemyState.Die;
            return;
        }

        // Hit로 전환
        Debug.Log($"상태전환: (any state, {CurrentState}) -> Hit");
        Debug.Log($"enemy : {Health}");
        CurrentState = EnemyState.Hit;
        if(_beingHit == null)
        {
            _beingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
        }
        else
        {
            StopCoroutine(_beingHit);
            _beingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
        }
    }
}
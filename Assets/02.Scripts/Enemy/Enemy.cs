using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 인공지능 : 지능을 가지고 행동하는 알고리즘
// 반응형 / 계획형 -> 규칙 기반 인공지능(전통적인 방식) 
//                 -> 제어문 기반

public class Enemy : MonoBehaviour
{
    public EnemyStatsSO Stat;
    public int Health;

    public EnemyState CurrentState = EnemyState.Idle;
    private CharacterController _characterController;
    private NavMeshAgent _agent;
    private Vector3 _returnPosition;
    public Transform[] PatrolPositions;

    private int _curPos;

    private GameObject _player;
    private float _attackTimer = 1f;
    private float _idleTimer = 0f;

    private Coroutine _beingHit;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
        _returnPosition = transform.position;
        Health = Stat.MaxHealth;
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
        if(_idleTimer >= Stat.IdleTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            BacklogUI.Instance.AddLog("적이 순찰합니다");
            CurrentState = EnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }

        // 가까워질 경우 Trace로 전환 
        if (Vector3.Distance(transform.position, _player.transform.position) < Stat.FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
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
            _agent.ResetPath();
            transform.position = PatrolPositions[_curPos].position;
            _returnPosition = PatrolPositions[_curPos].position;
            Debug.Log("상태전환: Patrol -> Idle");
            BacklogUI.Instance.AddLog("적이 멈췄습니다");
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
        _agent.SetDestination(PatrolPositions[_curPos].position);
        //Vector3 dir = (PatrolPositions[_curPos].position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Trace()
    {
        // 멀어질 경우 Return으로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) >= Stat.ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            BacklogUI.Instance.AddLog("적이 돌아갑니다");
            CurrentState = EnemyState.Return;
            return;
        }

        // 공격 범위만큼 가까워지면 Attack으로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) < Stat.AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            BacklogUI.Instance.AddLog("적이 당신을 공격합니다");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 플레이어 따라오기
        //Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        // 플레이어와 가까워질 경우 Trace로 전환
        if (Vector3.Distance(transform.position, _player.transform.position) < Stat.FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
            CurrentState = EnemyState.Trace;
        }

        // 원래 위치일 경우 Idle로 전환
        if (Vector3.Distance(transform.position, _returnPosition) <= 0.1f)
        {
            Debug.Log("상태전환: Return -> Idle");
            BacklogUI.Instance.AddLog("적이 멈췄습니다");
            transform.position = _returnPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 원래 위치로 복귀
        //Vector3 dir = (_returnPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_returnPosition);
    }

    private void Attack()
    {
        // 멀어질 경우 Trace로 전환
        if(Vector3.Distance(transform.position, _player.transform.position) >= Stat.AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
            CurrentState = EnemyState.Trace;
            _attackTimer = 1f;
            return;
        }

        // 플레이어를 공격
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= Stat.AttackCoolTime)
        {
            Debug.Log("플레이어를 공격합니다");
            BacklogUI.Instance.AddLog("공격을 받았습니다");
            _attackTimer = 0f;
        }

    }

    private IEnumerator Hit(Vector3 dir, float knockback)
    {
        // 일정 시간 경직
        float _timer = 0f;
        _agent.ResetPath();
        while(_timer <= Stat.HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // 상태 전환
        _agent.SetDestination(_player.transform.position);
        Debug.Log("상태전환: Hit -> Trace");
        BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(Stat.DieTime);
        gameObject.SetActive(false);
    }

    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Hit || CurrentState == EnemyState.Die) return;

        Health -= damage.Value;
        if(Health <= 0)
        {
            Debug.Log("상태전환: Hit -> Die");
            BacklogUI.Instance.AddLog("적이 죽었습니다");
            CurrentState = EnemyState.Die;
            return;
        }

        // Hit로 전환
        Debug.Log($"상태전환: (any state, {CurrentState}) -> Hit");
        BacklogUI.Instance.AddLog("적을 맞혔습니다");
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
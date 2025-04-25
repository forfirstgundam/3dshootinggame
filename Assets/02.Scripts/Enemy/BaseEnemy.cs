using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.XR;

//public enum EnemyState
//{
//    Idle,
//    Patrol,
//    Trace,
//    Return,
//    Attack,
//    Hit,
//    Die,
//}

public class BaseEnemy : MonoBehaviour
{
    public EnemyStatsSO Stat;
    public int Health;

    protected IEnemyState _currentState;
    protected CharacterController _characterController;
    protected NavMeshAgent _agent;
    protected Vector3 _returnPosition;
    public Vector3[] PatrolPositions;

    public int CurPatrolPos;

    public GameObject Player;

    protected Coroutine _beingHit;

    public void Initialize()
    {
        Health = Stat.MaxHealth;
    }

    public void ChangeEnemyState(IEnemyState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    public void EnemySetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }

    public void EnemyResetPath()
    {
        _agent.ResetPath();
    }

    public void EnemyKnockBack(Vector3 direction)
    {
        _characterController.Move(direction * Time.deltaTime);
    }

    public IEnumerator Hit(Vector3 dir, float knockback)
    {
        // ���� �ð� ����
        float _timer = 0f;
        _agent.ResetPath();
        while (_timer <= Stat.HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // ���� ��ȯ
        _agent.SetDestination(Player.transform.position);
        Debug.Log("������ȯ: Hit -> Trace");
        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    }
    public void TakeDamage(Damage damage)
    {
        if (_currentState is DieState) return;
        Health -= damage.Value;

        if (Health <= 0)
        {
            Debug.Log("������ȯ: Hit -> Die");
            BacklogUI.Instance.AddLog("���� �׾����ϴ�");
            ChangeEnemyState(new DieState());
            return;
        }
        else
        {
            ChangeEnemyState(new HitState(damage));
        }
    }

    //protected void Idle()
    //{
    //    // ���� �ð� ������ Patrol�� ��ȯ
    //    if (_idleTimer >= Stat.IdleTime)
    //    {
    //        Debug.Log("������ȯ: Idle -> Patrol");
    //        BacklogUI.Instance.AddLog("���� �����մϴ�");
    //        CurrentState = EnemyState.Patrol;
    //        _idleTimer = 0f;
    //        return;
    //    }

    //    // ������� ��� Trace�� ��ȯ 
    //    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.FindDistance)
    //    {
    //        Debug.Log("������ȯ: Idle -> Trace");
    //        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    //        CurrentState = EnemyState.Trace;
    //        _idleTimer = 0f;
    //        return;
    //    }

    //    _idleTimer += Time.deltaTime;
    //}

    //protected void Patrol()
    //{
    //    // ���� ��ġ�� �̵����� ��� : ���� ��ġ ����, �ٽ� idle��
    //    if (Vector3.Distance(transform.position, PatrolPositions[_curPos].position) <= 0.1f)
    //    {
    //        _agent.ResetPath();
    //        transform.position = PatrolPositions[_curPos].position;
    //        _returnPosition = PatrolPositions[_curPos].position;
    //        Debug.Log("������ȯ: Patrol -> Idle");
    //        BacklogUI.Instance.AddLog("���� ������ϴ�");
    //        CurrentState = EnemyState.Idle;
    //        if (_curPos >= PatrolPositions.Length - 1)
    //        {
    //            _curPos = 0;
    //        }
    //        else
    //        {
    //            _curPos++;
    //        }
    //        return;
    //    }

    //    // 3���� ��ġ�� �̵��ϱ�
    //    _agent.SetDestination(PatrolPositions[_curPos].position);
    //    //Vector3 dir = (PatrolPositions[_curPos].position - transform.position).normalized;
    //    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
    //}

    //protected void Trace()
    //{
    //    // �־��� ��� Return���� ��ȯ
    //    if (Vector3.Distance(transform.position, Player.transform.position) >= Stat.ReturnDistance)
    //    {
    //        Debug.Log("������ȯ: Trace -> Return");
    //        BacklogUI.Instance.AddLog("���� ���ư��ϴ�");
    //        CurrentState = EnemyState.Return;
    //        return;
    //    }

    //    // ���� ������ŭ ��������� Attack���� ��ȯ
    //    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.AttackDistance)
    //    {
    //        Debug.Log("������ȯ: Trace -> Attack");
    //        BacklogUI.Instance.AddLog("���� ����� �����մϴ�");
    //        CurrentState = EnemyState.Attack;
    //        return;
    //    }

    //    // �÷��̾� �������
    //    //Vector3 dir = (Player.transform.position - transform.position).normalized;
    //    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
    //    _agent.SetDestination(Player.transform.position);
    //}

    //protected void Return()
    //{
    //    // �÷��̾�� ������� ��� Trace�� ��ȯ
    //    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.FindDistance)
    //    {
    //        Debug.Log("������ȯ: Return -> Trace");
    //        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    //        CurrentState = EnemyState.Trace;
    //    }

    //    // ���� ��ġ�� ��� Idle�� ��ȯ
    //    if (Vector3.Distance(transform.position, _returnPosition) <= 0.1f)
    //    {
    //        Debug.Log("������ȯ: Return -> Idle");
    //        BacklogUI.Instance.AddLog("���� ������ϴ�");
    //        transform.position = _returnPosition;
    //        CurrentState = EnemyState.Idle;
    //        return;
    //    }

    //    // ���� ��ġ�� ����
    //    //Vector3 dir = (_returnPosition - transform.position).normalized;
    //    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
    //    _agent.SetDestination(_returnPosition);
    //}

    //protected void Attack()
    //{
    //    // �־��� ��� Trace�� ��ȯ
    //    if (Vector3.Distance(transform.position, Player.transform.position) >= Stat.AttackDistance)
    //    {
    //        Debug.Log("������ȯ: Attack -> Trace");
    //        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    //        CurrentState = EnemyState.Trace;
    //        _attackTimer = 1f;
    //        return;
    //    }

    //    // �÷��̾ ����
    //    _attackTimer += Time.deltaTime;
    //    if (_attackTimer >= Stat.AttackCoolTime)
    //    {
    //        Debug.Log("�÷��̾ �����մϴ�");
    //        BacklogUI.Instance.AddLog("������ �޾ҽ��ϴ�");
    //        _attackTimer = 0f;
    //    }

    //}

    //protected IEnumerator Hit(Vector3 dir, float knockback)
    //{
    //    // ���� �ð� ����
    //    float _timer = 0f;
    //    _agent.ResetPath();
    //    while (_timer <= Stat.HitTime)
    //    {
    //        _characterController.Move(dir * knockback * Time.deltaTime);
    //        _timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    // ���� ��ȯ
    //    _agent.SetDestination(Player.transform.position);
    //    Debug.Log("������ȯ: Hit -> Trace");
    //    BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    //    CurrentState = EnemyState.Trace;
    //}

    //protected IEnumerator Die()
    //{
    //    yield return new WaitForSeconds(Stat.DieTime);
    //    gameObject.SetActive(false);
    //}

    //public void TakeDamage(Damage damage)
    //{
    //    if (CurrentState == EnemyState.Hit || CurrentState == EnemyState.Die) return;

    //    Health -= damage.Value;
    //    if (Health <= 0)
    //    {
    //        Debug.Log("������ȯ: Hit -> Die");
    //        BacklogUI.Instance.AddLog("���� �׾����ϴ�");
    //        CurrentState = EnemyState.Die;
    //        return;
    //    }

    //    // Hit�� ��ȯ
    //    Debug.Log($"������ȯ: (any state, {CurrentState}) -> Hit");
    //    BacklogUI.Instance.AddLog("���� �������ϴ�");
    //    Debug.Log($"enemy : {Health}");
    //    CurrentState = EnemyState.Hit;
    //    if (_beingHit == null)
    //    {
    //        _beingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
    //    }
    //    else
    //    {
    //        StopCoroutine(_beingHit);
    //        _beingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
    //    }
    //}


}

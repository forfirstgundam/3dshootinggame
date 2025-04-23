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

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

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
        // ���� �ð� ������ Patrol�� ��ȯ
        if(_idleTimer >= IdleTime)
        {
            Debug.Log("������ȯ: Idle -> Patrol");
            BacklogUI.Instance.AddLog("���� �����մϴ�");
            CurrentState = EnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }

        // ������� ��� Trace�� ��ȯ 
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Idle -> Trace");
            BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
            CurrentState = EnemyState.Trace;
            _idleTimer = 0f;
            return;
        }

        _idleTimer += Time.deltaTime;
    }

    private void Patrol()
    {
        // ���� ��ġ�� �̵����� ��� : ���� ��ġ ����, �ٽ� idle��
        if (Vector3.Distance(transform.position, PatrolPositions[_curPos].position) <= 0.1f)
        {
            transform.position = PatrolPositions[_curPos].position;
            _returnPosition = PatrolPositions[_curPos].position;
            Debug.Log("������ȯ: Patrol -> Idle");
            BacklogUI.Instance.AddLog("���� ������ϴ�");
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

        // 3���� ��ġ�� �̵��ϱ�
        Vector3 dir = (PatrolPositions[_curPos].position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Trace()
    {
        // �־��� ��� Return���� ��ȯ
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("������ȯ: Trace -> Return");
            BacklogUI.Instance.AddLog("���� ���ư��ϴ�");
            CurrentState = EnemyState.Return;
            return;
        }

        // ���� ������ŭ ��������� Attack���� ��ȯ
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("������ȯ: Trace -> Attack");
            BacklogUI.Instance.AddLog("���� ����� �����մϴ�");
            CurrentState = EnemyState.Attack;
            return;
        }

        // �÷��̾� �������
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // �÷��̾�� ������� ��� Trace�� ��ȯ
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Return -> Trace");
            BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
            CurrentState = EnemyState.Trace;
        }

        // ���� ��ġ�� ��� Idle�� ��ȯ
        if (Vector3.Distance(transform.position, _returnPosition) <= 0.1f)
        {
            Debug.Log("������ȯ: Return -> Idle");
            BacklogUI.Instance.AddLog("���� ������ϴ�");
            transform.position = _returnPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // ���� ��ġ�� ����
        Vector3 dir = (_returnPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // �־��� ��� Trace�� ��ȯ
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("������ȯ: Attack -> Trace");
            BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
            CurrentState = EnemyState.Trace;
            _attackTimer = 1f;
            return;
        }

        // �÷��̾ ����
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCoolTime)
        {
            Debug.Log("�÷��̾ �����մϴ�");
            BacklogUI.Instance.AddLog("������ �޾ҽ��ϴ�");
            _attackTimer = 0f;
        }

    }

    private IEnumerator Hit(Vector3 dir, float knockback)
    {
        // ���� �ð� ����
        float _timer = 0f;
        while(_timer <= HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // ���� ��ȯ
        Debug.Log("������ȯ: Hit -> Trace");
        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
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
            Debug.Log("������ȯ: Hit -> Die");
            BacklogUI.Instance.AddLog("���� �׾����ϴ�");
            CurrentState = EnemyState.Die;
            return;
        }

        // Hit�� ��ȯ
        Debug.Log($"������ȯ: (any state, {CurrentState}) -> Hit");
        BacklogUI.Instance.AddLog("���� �������ϴ�");
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
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

// �ΰ����� : ������ ������ �ൿ�ϴ� �˰���
// ������ / ��ȹ�� -> ��Ģ ��� �ΰ�����(�������� ���) 
//                 -> ��� ���

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
        // ������ �ֱ�

        // ������� ��� Trace�� ��ȯ 
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {
        // �־��� ��� Return���� ��ȯ
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("������ȯ: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // ���� ������ŭ ��������� Attack���� ��ȯ
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("������ȯ: Trace -> Attack");
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
            CurrentState = EnemyState.Trace;
        }

        // ���� ��ġ�� ��� Idle�� ��ȯ
        if (Vector3.Distance(transform.position, _startPosition) <= 0.1f)
        {
            Debug.Log("������ȯ: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // ���� ��ġ�� ����
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // �־��� ��� Trace�� ��ȯ
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("������ȯ: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 1f;
            return;
        }

        // �÷��̾ ����
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCoolTime)
        {
            Debug.Log("�÷��̾ �����մϴ�");
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

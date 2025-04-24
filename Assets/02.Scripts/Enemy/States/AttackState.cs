using UnityEngine;

public class AttackState : IEnemyState
{
    private float _attackTimer;
    public void Enter(BaseEnemy enemy)
    {
        Debug.Log("���� �����߽��ϴ�!");
        // �÷��̾�� ����� �ִ� �� ����(���� ����)
        _attackTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        _attackTimer += Time.deltaTime;

        // ��Ÿ�Ӹ��� �÷��̾ ����
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("���� �����߽��ϴ�!");
            _attackTimer = 0f;
        }

        // ���ݹ������� �־��� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer > enemy.Stat.AttackDistance)
        {
            Debug.Log("���� ��ȭ : Attack -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}

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
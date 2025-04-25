using UnityEngine;

public class ReturnState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // ���� ��ġ�� ����
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos]);
    }

    public void Execute(BaseEnemy enemy)
    {
        // �÷��̾�� ������� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("���� ��ȭ : Return -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }

        // ���� ��ġ�� ���ƿ� ��� IdleState�� ��ȯ
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos]);
        if (distanceToPosition <= 0.1f)
        {
            Debug.Log("���� ��ȭ : Return -> Idle");
            enemy.ChangeEnemyState(new IdleState());
        }
    }
    public void Exit(BaseEnemy enemy)
    {
        enemy.EnemyResetPath();
    }
}

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

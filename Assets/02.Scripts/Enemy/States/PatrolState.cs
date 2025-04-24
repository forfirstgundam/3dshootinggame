using UnityEngine;

public class PatrolState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // PatrolPosition���� ����
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos].position);
    }

    public void Execute(BaseEnemy enemy)
    {
        // ���� PatrolPosition�� �������� ��� IdleState�� ��ȯ
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos].position);
        if (distanceToPosition <= 0.1f)
        {
            Debug.Log("���� ��ȭ : Patrol -> Idle");
            enemy.ChangeEnemyState(new IdleState());
            if (enemy.CurPatrolPos >= enemy.PatrolPositions.Length - 1)
            {
                enemy.CurPatrolPos = 0;
            }
            else
            {
                enemy.CurPatrolPos++;
            }
        }

        // Player�� ������� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("���� ��ȭ : Patrol -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.EnemyResetPath();
    }
}

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
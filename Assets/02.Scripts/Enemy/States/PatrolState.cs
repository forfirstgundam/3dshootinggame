using UnityEngine;

public class PatrolState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // PatrolPosition으로 가기
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos].position);
    }

    public void Execute(BaseEnemy enemy)
    {
        // 현재 PatrolPosition에 도착했을 경우 IdleState로 전환
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos].position);
        if (distanceToPosition <= 0.1f)
        {
            Debug.Log("상태 변화 : Patrol -> Idle");
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

        // Player가 가까워질 경우 TraceState로 전환
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("상태 변화 : Patrol -> Trace");
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
//    // 지정 위치로 이동했을 경우 : 다음 위치 지정, 다시 idle로
//    if (Vector3.Distance(transform.position, PatrolPositions[_curPos].position) <= 0.1f)
//    {
//        _agent.ResetPath();
//        transform.position = PatrolPositions[_curPos].position;
//        _returnPosition = PatrolPositions[_curPos].position;
//        Debug.Log("상태전환: Patrol -> Idle");
//        BacklogUI.Instance.AddLog("적이 멈췄습니다");
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

//    // 3가지 위치로 이동하기
//    _agent.SetDestination(PatrolPositions[_curPos].position);
//    //Vector3 dir = (PatrolPositions[_curPos].position - transform.position).normalized;
//    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
//}
using UnityEngine;

public class ReturnState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // 원래 위치로 복귀
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos]);
    }

    public void Execute(BaseEnemy enemy)
    {
        // 플레이어와 가까워질 경우 TraceState로 전환
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("상태 변화 : Return -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }

        // 원래 위치로 돌아올 경우 IdleState로 전환
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos]);
        if (distanceToPosition <= 0.1f)
        {
            Debug.Log("상태 변화 : Return -> Idle");
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
//    // 플레이어와 가까워질 경우 Trace로 전환
//    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.FindDistance)
//    {
//        Debug.Log("상태전환: Return -> Trace");
//        BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
//        CurrentState = EnemyState.Trace;
//    }

//    // 원래 위치일 경우 Idle로 전환
//    if (Vector3.Distance(transform.position, _returnPosition) <= 0.1f)
//    {
//        Debug.Log("상태전환: Return -> Idle");
//        BacklogUI.Instance.AddLog("적이 멈췄습니다");
//        transform.position = _returnPosition;
//        CurrentState = EnemyState.Idle;
//        return;
//    }

//    // 원래 위치로 복귀
//    //Vector3 dir = (_returnPosition - transform.position).normalized;
//    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
//    _agent.SetDestination(_returnPosition);
//}

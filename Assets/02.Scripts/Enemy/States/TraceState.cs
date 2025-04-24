using UnityEngine;

public class TraceState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // 플레이어 따라가기
        enemy.EnemySetDestination(enemy.Player.transform.position);
    }

    public void Execute(BaseEnemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        // 공격 범위만큼 가까워지면 AttackState로 전환
        if(distanceToPlayer <= enemy.Stat.AttackDistance)
        {
            Debug.Log("상태 변화 : Trace -> Attack");
            enemy.ChangeEnemyState(new AttackState());
        }

        // 복귀 범위보다 멀어지면 ReturnState로 전환
        if (distanceToPlayer > enemy.Stat.ReturnDistance)
        {
            Debug.Log("상태 변화 : Trace -> Return");
            enemy.ChangeEnemyState(new ReturnState());
        }

        // 플레이어 위치를 계속 따라감
        enemy.EnemySetDestination(enemy.Player.transform.position);
    }
    public void Exit(BaseEnemy enemy)
    {
        enemy.EnemyResetPath();
    }
}

//protected void Trace()
//{
//    // 멀어질 경우 Return으로 전환
//    if (Vector3.Distance(transform.position, Player.transform.position) >= Stat.ReturnDistance)
//    {
//        Debug.Log("상태전환: Trace -> Return");
//        BacklogUI.Instance.AddLog("적이 돌아갑니다");
//        CurrentState = EnemyState.Return;
//        return;
//    }

//    // 공격 범위만큼 가까워지면 Attack으로 전환
//    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.AttackDistance)
//    {
//        Debug.Log("상태전환: Trace -> Attack");
//        BacklogUI.Instance.AddLog("적이 당신을 공격합니다");
//        CurrentState = EnemyState.Attack;
//        return;
//    }

//    // 플레이어 따라오기
//    //Vector3 dir = (Player.transform.position - transform.position).normalized;
//    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
//    _agent.SetDestination(Player.transform.position);
//}

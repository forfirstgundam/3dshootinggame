using UnityEngine;

public class PatrolState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // PatrolPosition으로 가기
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos]);
    }

    public void Execute(BaseEnemy enemy)
    {
        // 현재 PatrolPosition에 도착했을 경우 IdleState로 전환
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos]);
        if (distanceToPosition <= 0.5f)
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
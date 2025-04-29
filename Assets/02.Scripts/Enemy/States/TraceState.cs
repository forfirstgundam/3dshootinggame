using UnityEngine;

public class TraceState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        // 플레이어 따라가기
        enemy.EnemySetDestination(enemy.Player.transform.position);

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
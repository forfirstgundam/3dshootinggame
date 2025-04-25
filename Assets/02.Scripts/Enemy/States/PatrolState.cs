using UnityEngine;

public class PatrolState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // PatrolPosition���� ����
        enemy.EnemySetDestination(enemy.PatrolPositions[enemy.CurPatrolPos]);
    }

    public void Execute(BaseEnemy enemy)
    {
        // ���� PatrolPosition�� �������� ��� IdleState�� ��ȯ
        float distanceToPosition = Vector3.Distance(enemy.transform.position, enemy.PatrolPositions[enemy.CurPatrolPos]);
        if (distanceToPosition <= 0.5f)
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
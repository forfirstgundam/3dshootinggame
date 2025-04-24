using UnityEngine;

public class IdleState : IEnemyState
{
    private float _idleTimer;

    public void Enter(BaseEnemy enemy)
    {
        _idleTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        _idleTimer += Time.deltaTime;

        // ���� �ð� ���� ��� PatrolState�� ��ȯ
        if (_idleTimer >= enemy.Stat.IdleTime)
        {
            Debug.Log("���� ��ȭ : Idle -> Patrol");
            enemy.ChangeEnemyState(new PatrolState());
        }

        // Player�� ������� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("���� ��ȭ : Idle -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }

    public void Exit(BaseEnemy enemy)
    {

    }
}

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
        if (GameManager.Instance.GameState != GameState.Play) return;
        _idleTimer += Time.deltaTime;

        // ���� �ð� ���� ��� PatrolState�� ��ȯ
        if (_idleTimer >= enemy.Stat.IdleTime)
        {
            Debug.Log("���� ��ȭ : Idle -> Patrol");
            enemy.ChangeEnemyState(new PatrolState());
            enemy.Animator.SetTrigger("IdleToMove");
        }

        // Player�� ������� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.PlayerGameObject.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("���� ��ȭ : Idle -> Trace");
            enemy.ChangeEnemyState(new TraceState());
            enemy.Animator.SetTrigger("IdleToMove");
        }
    }

    public void Exit(BaseEnemy enemy)
    {

    }
}

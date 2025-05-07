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

        // 일정 시간 지날 경우 PatrolState로 전환
        if (_idleTimer >= enemy.Stat.IdleTime)
        {
            Debug.Log("상태 변화 : Idle -> Patrol");
            enemy.ChangeEnemyState(new PatrolState());
            enemy.Animator.SetTrigger("IdleToMove");
        }

        // Player가 가까워질 경우 TraceState로 전환
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.PlayerGameObject.transform.position);
        if (distanceToPlayer < enemy.Stat.FindDistance)
        {
            Debug.Log("상태 변화 : Idle -> Trace");
            enemy.ChangeEnemyState(new TraceState());
            enemy.Animator.SetTrigger("IdleToMove");
        }
    }

    public void Exit(BaseEnemy enemy)
    {

    }
}

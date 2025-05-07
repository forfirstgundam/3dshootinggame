using UnityEngine;

public class MimicIdleState : IEnemyState
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
    }

    public void Exit(BaseEnemy enemy)
    {
        MimicEnemy mimic = enemy.gameObject.GetComponent<MimicEnemy>();
        enemy.Animator.SetTrigger("Uncovered");
        mimic.Canvas.SetActive(true);
    }
}

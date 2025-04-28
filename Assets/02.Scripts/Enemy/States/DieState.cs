using UnityEngine;

public class DieState : IEnemyState
{
    private float _dieTimer;

    public void Enter(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        enemy.EnemyResetPath();
        _dieTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        _dieTimer += Time.deltaTime;

        if (_dieTimer >= enemy.Stat.DieTime)
        {
            Debug.Log("적이 죽었습니다");
            enemy.gameObject.SetActive(false);
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}

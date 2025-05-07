using UnityEngine;

public class DieState : IEnemyState
{
    private float _dieTimer;
    private bool _droppedCoins = false;

    public void Enter(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        enemy.EnemyResetPath();
        _dieTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        _dieTimer += Time.deltaTime;

        if (!_droppedCoins)
        {
            for (int i = 0; i < enemy.Stat.DropCoins; i++)
            {
                Vector3 randpos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), Random.Range(-0.5f, 0.5f));
                Pools.Instance.Create(3, enemy.transform.position + randpos);
            }
            _droppedCoins = true;
        }

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

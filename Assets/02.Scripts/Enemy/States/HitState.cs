using UnityEngine;

public class HitState : IEnemyState
{
    private Damage _damage;
    private float _timer;

    public HitState(Damage damage)
    {
        _damage = damage;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.EnemyResetPath();
        _timer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        _timer += Time.deltaTime;

        // �˹� ó��
        enemy.EnemyKnockBack(_damage.KnockDir * _damage.KnockValue);

        // �˹��� ���� �� TraceState�� ��ȯ
        if (_timer >= enemy.Stat.HitTime)
        {
            Debug.Log("���� ��ȭ : Hit -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        
    }
}
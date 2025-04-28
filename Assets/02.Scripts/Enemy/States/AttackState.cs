using UnityEngine;

public class AttackState : IEnemyState
{
    private float _attackTimer;
    public void Enter(BaseEnemy enemy)
    {
        Debug.Log("적이 공격했습니다!");
        // 플레이어에게 대미지 주는 것 구현(아직 없음)
        _attackTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        _attackTimer += Time.deltaTime;
        Damage damage = new Damage();
        damage.Value = enemy.Stat.AttackDamage;
        damage.KnockValue = enemy.Stat.AttackKnockValue;
        damage.From = enemy.gameObject;

        // 쿨타임마다 플레이어를 공격
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("적이 공격했습니다!");
            IDamageable player = enemy.Player.GetComponent<IDamageable>();
            damage.KnockDir = (enemy.Player.transform.position - enemy.transform.position).normalized;

            player.TakeDamage(damage);

            _attackTimer = 0f;
        }

        // 공격범위보다 멀어질 경우 TraceState로 전환
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer > enemy.Stat.AttackDistance)
        {
            Debug.Log("상태 변화 : Attack -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}
using UnityEngine;

public class AttackState : IEnemyState
{
    private float _attackTimer;
    public void Enter(BaseEnemy enemy)
    {
        Debug.Log("적이 공격했습니다!");
        _attackTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;

        // ChargeEnemy가 2번 공격했을 경우
        if (enemy.TryGetComponent<ChargeEnemy>(out ChargeEnemy charger))
        {
            if(charger.AttackCount >= 2)
            {
                Debug.Log("상태 변화 : Attack -> Charge");
                charger.AttackCount = 0;
                enemy.ChangeEnemyState(new ChargeState());
            }
        }

        _attackTimer += Time.deltaTime;
        Damage damage = new Damage();
        damage.Value = enemy.Stat.AttackDamage;
        damage.KnockValue = enemy.Stat.AttackKnockValue;
        damage.From = enemy.gameObject;

        // 쿨타임마다 플레이어를 공격
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("적이 공격했습니다!");
            enemy.EnemyResetPath();
            enemy.transform.LookAt(Player.Instance.transform);
            enemy.Animator.SetTrigger("AttackDelayToAttack");

            // ChargeEnemy의 경우 2번 공격 후 Charge공격
            if(charger != null)
            {
                charger.AttackCount++;
                Debug.Log($"Attack Count is {charger.AttackCount}");
            }

            IDamageable player = enemy.PlayerGameObject.GetComponent<IDamageable>();
            damage.KnockDir = (enemy.PlayerGameObject.transform.position - enemy.transform.position).normalized;

            player.TakeDamage(damage);

            _attackTimer = 0f;
        }

        // 공격범위보다 멀어질 경우 TraceState로 전환
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.PlayerGameObject.transform.position);
        if (distanceToPlayer > enemy.Stat.AttackDistance)
        {
            Debug.Log("상태 변화 : Attack -> Trace");
            enemy.ChangeEnemyState(new TraceState());
            enemy.Animator.SetTrigger("AttackDelayToMove");
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}
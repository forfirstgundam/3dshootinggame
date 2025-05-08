using UnityEngine;

public class AttackState : IEnemyState
{
    private float _attackTimer;
    public void Enter(BaseEnemy enemy)
    {
        Debug.Log("���� �����߽��ϴ�!");
        _attackTimer = 0f;
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;

        // ChargeEnemy�� 2�� �������� ���
        if (enemy.TryGetComponent<ChargeEnemy>(out ChargeEnemy charger))
        {
            if(charger.AttackCount >= 2)
            {
                Debug.Log("���� ��ȭ : Attack -> Charge");
                charger.AttackCount = 0;
                enemy.ChangeEnemyState(new ChargeState());
            }
        }

        _attackTimer += Time.deltaTime;
        Damage damage = new Damage();
        damage.Value = enemy.Stat.AttackDamage;
        damage.KnockValue = enemy.Stat.AttackKnockValue;
        damage.From = enemy.gameObject;

        // ��Ÿ�Ӹ��� �÷��̾ ����
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("���� �����߽��ϴ�!");
            enemy.EnemyResetPath();
            enemy.transform.LookAt(Player.Instance.transform);
            enemy.Animator.SetTrigger("AttackDelayToAttack");

            // ChargeEnemy�� ��� 2�� ���� �� Charge����
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

        // ���ݹ������� �־��� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.PlayerGameObject.transform.position);
        if (distanceToPlayer > enemy.Stat.AttackDistance)
        {
            Debug.Log("���� ��ȭ : Attack -> Trace");
            enemy.ChangeEnemyState(new TraceState());
            enemy.Animator.SetTrigger("AttackDelayToMove");
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}
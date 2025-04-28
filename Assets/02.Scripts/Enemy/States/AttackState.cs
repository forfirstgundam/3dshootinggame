using UnityEngine;

public class AttackState : IEnemyState
{
    private float _attackTimer;
    public void Enter(BaseEnemy enemy)
    {
        Debug.Log("���� �����߽��ϴ�!");
        // �÷��̾�� ����� �ִ� �� ����(���� ����)
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

        // ��Ÿ�Ӹ��� �÷��̾ ����
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("���� �����߽��ϴ�!");
            IDamageable player = enemy.Player.GetComponent<IDamageable>();
            damage.KnockDir = (enemy.Player.transform.position - enemy.transform.position).normalized;

            player.TakeDamage(damage);

            _attackTimer = 0f;
        }

        // ���ݹ������� �־��� ��� TraceState�� ��ȯ
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distanceToPlayer > enemy.Stat.AttackDistance)
        {
            Debug.Log("���� ��ȭ : Attack -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }
    public void Exit(BaseEnemy enemy)
    {

    }
}
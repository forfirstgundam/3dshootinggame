using UnityEngine;

public class TraceState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        // �÷��̾� ���󰡱�
        enemy.EnemySetDestination(enemy.Player.transform.position);

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        // ���� ������ŭ ��������� AttackState�� ��ȯ
        if(distanceToPlayer <= enemy.Stat.AttackDistance)
        {
            Debug.Log("���� ��ȭ : Trace -> Attack");
            enemy.ChangeEnemyState(new AttackState());
        }

        // ���� �������� �־����� ReturnState�� ��ȯ
        if (distanceToPlayer > enemy.Stat.ReturnDistance)
        {
            Debug.Log("���� ��ȭ : Trace -> Return");
            enemy.ChangeEnemyState(new ReturnState());
        }

        // �÷��̾� ��ġ�� ��� ����
        enemy.EnemySetDestination(enemy.Player.transform.position);
    }
    public void Exit(BaseEnemy enemy)
    {
        enemy.EnemyResetPath();
    }
}
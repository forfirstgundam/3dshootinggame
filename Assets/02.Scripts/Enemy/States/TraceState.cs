using UnityEngine;

public class TraceState : IEnemyState
{
    public void Enter(BaseEnemy enemy)
    {
        // �÷��̾� ���󰡱�
        enemy.EnemySetDestination(enemy.Player.transform.position);
    }

    public void Execute(BaseEnemy enemy)
    {
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

//protected void Trace()
//{
//    // �־��� ��� Return���� ��ȯ
//    if (Vector3.Distance(transform.position, Player.transform.position) >= Stat.ReturnDistance)
//    {
//        Debug.Log("������ȯ: Trace -> Return");
//        BacklogUI.Instance.AddLog("���� ���ư��ϴ�");
//        CurrentState = EnemyState.Return;
//        return;
//    }

//    // ���� ������ŭ ��������� Attack���� ��ȯ
//    if (Vector3.Distance(transform.position, Player.transform.position) < Stat.AttackDistance)
//    {
//        Debug.Log("������ȯ: Trace -> Attack");
//        BacklogUI.Instance.AddLog("���� ����� �����մϴ�");
//        CurrentState = EnemyState.Attack;
//        return;
//    }

//    // �÷��̾� �������
//    //Vector3 dir = (Player.transform.position - transform.position).normalized;
//    //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
//    _agent.SetDestination(Player.transform.position);
//}

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

//protected IEnumerator Hit(Vector3 dir, float knockback)
//{
//    // ���� �ð� ����
//    float _timer = 0f;
//    _agent.ResetPath();
//    while (_timer <= Stat.HitTime)
//    {
//        _characterController.Move(dir * knockback * Time.deltaTime);
//        _timer += Time.deltaTime;
//        yield return null;
//    }

//    // ���� ��ȯ
//    _agent.SetDestination(Player.transform.position);
//    Debug.Log("������ȯ: Hit -> Trace");
//    BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
//    CurrentState = EnemyState.Trace;
//}
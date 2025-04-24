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
        _attackTimer += Time.deltaTime;

        // 쿨타임마다 플레이어를 공격
        if (_attackTimer >= enemy.Stat.AttackCoolTime)
        {
            Debug.Log("적이 공격했습니다!");
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

//protected void Attack()
//{
//    // 멀어질 경우 Trace로 전환
//    if (Vector3.Distance(transform.position, Player.transform.position) >= Stat.AttackDistance)
//    {
//        Debug.Log("상태전환: Attack -> Trace");
//        BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
//        CurrentState = EnemyState.Trace;
//        _attackTimer = 1f;
//        return;
//    }

//    // 플레이어를 공격
//    _attackTimer += Time.deltaTime;
//    if (_attackTimer >= Stat.AttackCoolTime)
//    {
//        Debug.Log("플레이어를 공격합니다");
//        BacklogUI.Instance.AddLog("공격을 받았습니다");
//        _attackTimer = 0f;
//    }

//}
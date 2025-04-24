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

        // 넉백 처리
        enemy.EnemyKnockBack(_damage.KnockDir * _damage.KnockValue);

        // 넉백이 끝난 후 TraceState로 전환
        if (_timer >= enemy.Stat.HitTime)
        {
            Debug.Log("상태 변화 : Hit -> Trace");
            enemy.ChangeEnemyState(new TraceState());
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        
    }
}

//protected IEnumerator Hit(Vector3 dir, float knockback)
//{
//    // 일정 시간 경직
//    float _timer = 0f;
//    _agent.ResetPath();
//    while (_timer <= Stat.HitTime)
//    {
//        _characterController.Move(dir * knockback * Time.deltaTime);
//        _timer += Time.deltaTime;
//        yield return null;
//    }

//    // 상태 전환
//    _agent.SetDestination(Player.transform.position);
//    Debug.Log("상태전환: Hit -> Trace");
//    BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
//    CurrentState = EnemyState.Trace;
//}
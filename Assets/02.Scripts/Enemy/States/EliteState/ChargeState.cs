using UnityEngine;
using System.Collections;

public class ChargeState : IEnemyState
{
    private ChargeEnemy _charger;
    private Vector3 _chargePosition;
    private Coroutine _isPreparing;

    public void Enter(BaseEnemy enemy)
    {
        _charger = enemy.gameObject.GetComponent<ChargeEnemy>();
        _charger.ChangeToChargeSpeed();
    }

    public void Execute(BaseEnemy enemy)
    {
        if (GameManager.Instance.GameState != GameState.Play) return;

        if (_charger.CanCharge)
        {
            float distanceToCharge = Vector3.Distance(enemy.transform.position, _chargePosition);
            if (distanceToCharge <= 0.5f)
            {
                // 플레이어와 충돌했을 경우 대미지를 주고 밀어냄 - chargeenemy에서

                // TraceState로 전환
                enemy.ChangeEnemyState(new TraceState());
            }

            enemy.EnemySetDestination(_chargePosition);
        }

        enemy.EnemyResetPath();
        
        if(_isPreparing == null)
        {
            _isPreparing = enemy.StartCoroutine(SaveUpForCharge());
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        _charger.ChangeToNormalSpeed();
    }

    private IEnumerator SaveUpForCharge()
    {
        yield return new WaitForSeconds(2f);
        _chargePosition = Player.Instance.transform.position;
        _charger.CanCharge = true;
    }
}

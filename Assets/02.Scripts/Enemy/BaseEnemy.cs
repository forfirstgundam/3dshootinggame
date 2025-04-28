using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public EnemyStatsSO Stat;
    public int Health;

    protected IEnemyState _currentState;
    protected CharacterController _characterController;
    protected NavMeshAgent _agent;
    protected Vector3 _returnPosition;
    public Vector3[] PatrolPositions;

    public int CurPatrolPos;

    public GameObject Player;

    protected Coroutine _beingHit;
    public Action<int> OnEnemyHit;

    public void Initialize()
    {
        Health = Stat.MaxHealth;
    }

    public void ChangeEnemyState(IEnemyState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    public void EnemySetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }

    public void EnemyResetPath()
    {
        _agent.ResetPath();
    }

    public void EnemyKnockBack(Vector3 direction)
    {
        _characterController.Move(direction * Time.deltaTime);
    }

    public IEnumerator Hit(Vector3 dir, float knockback)
    {
        // 일정 시간 경직
        float _timer = 0f;
        _agent.ResetPath();
        while (_timer <= Stat.HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // 상태 전환
        _agent.SetDestination(Player.transform.position);
        Debug.Log("상태전환: Hit -> Trace");
        BacklogUI.Instance.AddLog("적이 당신을 알아챘습니다");
    }

    public void TakeDamage(Damage damage)
    {
        if (_currentState is DieState) return;
        Health -= damage.Value;
        OnEnemyHit?.Invoke(Health);

        if (Health <= 0)
        {
            Debug.Log("상태전환: Hit -> Die");
            BacklogUI.Instance.AddLog("적이 죽었습니다");
            ChangeEnemyState(new DieState());
            return;
        }
        else
        {
            ChangeEnemyState(new HitState(damage));
        }
    }
}

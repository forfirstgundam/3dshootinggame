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
        // ���� �ð� ����
        float _timer = 0f;
        _agent.ResetPath();
        while (_timer <= Stat.HitTime)
        {
            _characterController.Move(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }

        // ���� ��ȯ
        _agent.SetDestination(Player.transform.position);
        Debug.Log("������ȯ: Hit -> Trace");
        BacklogUI.Instance.AddLog("���� ����� �˾�ë���ϴ�");
    }

    public void TakeDamage(Damage damage)
    {
        if (_currentState is DieState) return;
        Health -= damage.Value;
        OnEnemyHit?.Invoke(Health);

        if (Health <= 0)
        {
            Debug.Log("������ȯ: Hit -> Die");
            BacklogUI.Instance.AddLog("���� �׾����ϴ�");
            ChangeEnemyState(new DieState());
            return;
        }
        else
        {
            ChangeEnemyState(new HitState(damage));
        }
    }
}

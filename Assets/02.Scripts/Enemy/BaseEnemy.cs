using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public EnemyStatsSO Stat;
    public int Health;

    protected IEnemyState _currentState;
    protected CharacterController _characterController;
    protected NavMeshAgent _agent;
    protected Vector3 _returnPosition;
    public Vector3[] PatrolPositions;
    public Animator Animator;

    public int CurPatrolPos;

    public GameObject Player;

    protected Coroutine _beingHit;
    protected Coroutine _isFlashing;
    public Action<int> OnEnemyHit;

    private List<Material> _flashingMats = new List<Material>();
    private List<Color> _originalColors = new List<Color>();

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

    public IEnumerator FlashRed()
    {
        _flashingMats.Clear();
        _originalColors.Clear();

        foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            foreach (var mat in renderer.materials)
            {
                if (mat.HasProperty("_BaseColor"))
                {
                    _flashingMats.Add(mat);
                    _originalColors.Add(mat.GetColor("_BaseColor"));
                    mat.SetColor("_BaseColor", Color.red);
                }
            }
        }

        yield return ResetMaterialColors();
    }

    public IEnumerator ResetMaterialColors()
    {
        yield return new WaitForSeconds(Stat.HitTime);

        for (int i = 0; i < _flashingMats.Count; i++)
        {
            _flashingMats[i].SetColor("_BaseColor", _originalColors[i]);
        }

        _isFlashing = null;
    }

    public void TakeDamage(Damage damage)
    {
        if (_currentState is DieState) return;
        Health -= damage.Value;
        OnEnemyHit?.Invoke(Health);
        Animator.SetTrigger("Hit");
        if(_isFlashing == null)
        {
            _isFlashing = StartCoroutine(FlashRed());
        }

        if (Health <= 0)
        {
            Debug.Log("상태전환: Hit -> Die");
            BacklogUI.Instance.AddLog("적이 죽었습니다");
            ChangeEnemyState(new DieState());
            Animator.SetTrigger("Die");
            return;
        }
        else
        {
            ChangeEnemyState(new HitState(damage));
        }
    }
}

using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerStatsSO Stat;
    public int Health;
    public Action<int> OnPlayerHit;
    public Action OnPlayerDeath;
    private Animator _animator;

    private void Awake()
    {
        Health = Stat.MaxHealth;
        _animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        OnPlayerHit?.Invoke(Health);
        float weight = (float)Health / (float)Stat.MaxHealth;
        int injuredLayerIndex = _animator.GetLayerIndex("Injured Layer");
        _animator.SetLayerWeight(injuredLayerIndex, 1f - weight);

        if (Health <= 0)
        {
            OnPlayerDeath?.Invoke();
            Debug.Log("you died!!");
        }
        MainUI.Instance.GlitchEffectOn();
    }
}

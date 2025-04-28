using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerStatsSO Stat;
    public int Health;
    public Action<int> OnPlayerHit;
    public Action OnPlayerDeath;

    private void Awake()
    {
        Health = Stat.MaxHealth;
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        OnPlayerHit?.Invoke(Health);
        if(Health <= 0)
        {
            OnPlayerDeath?.Invoke();
            Debug.Log("you died!!");
        }
        MainUI.Instance.GlitchEffectOn();
    }
}

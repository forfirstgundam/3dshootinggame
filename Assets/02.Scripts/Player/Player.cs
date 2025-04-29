using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;

    public PlayerStatsSO Stat;
    public int Health;
    public Action<int> OnPlayerHit;
    public Action OnPlayerDeath;
    public Animator CurrentAnimator;

    private void Awake()
    {
        Instance = this;
        Health = Stat.MaxHealth;
        CurrentAnimator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        OnPlayerHit?.Invoke(Health);
        float weight = (float)Health / (float)Stat.MaxHealth;
        int injuredLayerIndex = CurrentAnimator.GetLayerIndex("Injured Layer");
        CurrentAnimator.SetLayerWeight(injuredLayerIndex, 1f - weight);

        if (Health <= 0)
        {
            OnPlayerDeath?.Invoke();
            Debug.Log("you died!!");
        }
        MainUI.Instance.GlitchEffectOn();
    }
}

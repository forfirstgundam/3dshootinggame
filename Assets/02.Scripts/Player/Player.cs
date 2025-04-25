using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerStatsSO Stat;
    public int Health;

    private void Awake()
    {
        Health = Stat.MaxHealth;
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if(Health <= 0)
        {
            Debug.Log("you died!!");
        }
        MainUI.Instance.GlitchEffectOn();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    public float MoveSpeed = 5f;
    public float FindDistance = 7f;
    public float ReturnDistance = 15f;
    public float AttackDistance = 3f;

    public float AttackCoolTime = 1f;
    public int AttackDamage = 0;
    public float AttackKnockValue = 0f;

    public float IdleTime = 5f;
    public float HitTime = 0.5f;
    public float DieTime = 1f;

    public int MaxHealth = 100;
}

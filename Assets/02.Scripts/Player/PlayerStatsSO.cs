using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    public int MaxHealth;
    public float MaxStamina = 50f;

    public float Stamina = 50f;
    public float FillRate = 5f;
    public float DashUseRate = 10f;
    public float ClimbUseRate = 5f;

    public float RollUsage = 20f;
}

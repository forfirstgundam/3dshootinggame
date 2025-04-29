using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatsSO", menuName = "Scriptable Objects/WeaponStatsSO")]
public class WeaponStatsSO : ScriptableObject
{
    [Header("General Stats")]
    public Weapon WeaponType;
    public int DamageValue;
    public float KnockValue;

    [Header("Gun")]
    public float BulletCoolTime = 0.1f;
    public float LoadTime = 2f;
    public int MaxBullet = 50;

    [Header("Sword")]
    public float SwingCoolTime = 0.5f;
    public float SwingRange = 2f;
    public float SwingAngle = 100f;


    [Header("Bomb")]
    public int MaxBomb = 3;
    public float MinThrowPower = 15f;
    public float MaxThrowPower = 30f;
    public float ThrowPowerRate = 10f;
}

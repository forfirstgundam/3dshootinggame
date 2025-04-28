using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Slider HealthBar;
    public int MaxHealth;

    public EnemyStatsSO Stat;

    private void Start()
    {
        BaseEnemy enemy = GetComponent<BaseEnemy>();
        MaxHealth = Stat.MaxHealth;
        HealthBar.value = 1f;
        enemy.OnEnemyHit += UpdateEnemyHealth;
    }

    public void UpdateEnemyHealth(int hp)
    {
        HealthBar.value = (float)hp / (float)MaxHealth;
        Debug.Log($"Health is {hp}, maxhealth is {MaxHealth}");
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyUI_elites : MonoBehaviour
{
    public Slider HealthBar;
    public Slider DelayedBar;
    public int MaxHealth;
    public GameObject Canvas;

    public EnemyStatsSO Stat;
    private float targetValue;
    public float DelayTime = 0.4f;
    public float DelaySpeed = 4f;

    private Coroutine _delayCoroutine;

    private void Start()
    {
        BaseEnemy enemy = GetComponent<BaseEnemy>();
        MaxHealth = Stat.MaxHealth;
        HealthBar.value = 1f;
        DelayedBar.value = 1f;
        enemy.OnEnemyHit += UpdateEnemyHealth;
    }

    public void UpdateEnemyHealth(int hp)
    {
        targetValue = (float)hp / (float)MaxHealth;
        Debug.Log($"Health is {hp} / {MaxHealth}");
        HealthBar.value = targetValue; // 즉시 반영

        if (_delayCoroutine != null) StopCoroutine(_delayCoroutine);
        _delayCoroutine = StartCoroutine(DelayedBarUpdate());
    }
    private IEnumerator DelayedBarUpdate()
    {
        yield return new WaitForSeconds(DelayTime);

        while (DelayedBar.value > targetValue)
        {
            DelayedBar.value = Mathf.MoveTowards(DelayedBar.value, targetValue, Time.deltaTime * DelaySpeed);
            yield return null;
        }

        _delayCoroutine = null;
    }

    private void Update()
    {
        Canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}

using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Barrel : MonoBehaviour
{
    public int MaxHealth = 50;
    public int CurHealth;

    public float HitTime = 0.05f;
    public float FlyTime = 5f;

    public float ExplodePower = 50f;

    private Coroutine BeingHit;

    public void Initialize()
    {
        CurHealth = MaxHealth;
    }

    protected IEnumerator Hit(Vector3 dir, float knockback)
    {
        // 일정 시간 경직
        float _timer = 0f;
        Debug.Log("barrel has been hit");
        while (_timer <= HitTime)
        {
            transform.Translate(dir * knockback * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
    }

    protected IEnumerator FlyAway(Vector3 dir, float knockback)
    {
        float _timer = 0f;
        dir.y += ExplodePower;
        dir = dir.normalized;
        while (_timer <= FlyTime)
        {
            transform.Translate(dir * knockback * ExplodePower * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
    }

    public void TakeDamage(Damage damage)
    {
        CurHealth -= damage.Value;
        if (CurHealth <= 0)
        {
            Debug.Log("barrel exploded boom");
            StartCoroutine(FlyAway(damage.KnockDir, damage.KnockValue));
        }
        else
        {
            if (BeingHit == null)
            {
                BeingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
            }
            else
            {
                StopCoroutine(BeingHit);
                BeingHit = StartCoroutine(Hit(damage.KnockDir, damage.KnockValue));
            }
        }
    }
}

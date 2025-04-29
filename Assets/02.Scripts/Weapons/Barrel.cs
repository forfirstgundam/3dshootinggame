using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Barrel : MonoBehaviour, IDamageable
{
    public int MaxHealth = 50;
    public int CurHealth;
    public ParticleSystem Explosion;

    public float HitTime = 0.05f;
    public float FlyTime = 5f;

    public float ExplodePower = 50f;
    public float ExplodePushPower = 0.2f;
    public float ExplosionRadius = 3f;
    public int ExplodeDamage = 50;

    private Coroutine BeingHit;
    private Coroutine Flying;
    private bool Exploding = false;

    private void Awake()
    {
        Initialize();
    }
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

    private IEnumerator FlyAway(Vector3 dir, float knockback)
    {
        float _timer = 0f;
        dir.y += ExplodePower;
        dir = dir.normalized;
        Instantiate(Explosion, transform.position, Quaternion.identity);

        Damage damage = new Damage();
        damage.Value = 50;
        damage.From = this.gameObject;
        damage.KnockValue = 0.2f;

        Exploding = true;
        Explode();

        while (_timer <= FlyTime)
        {
            transform.Translate(dir * knockback * ExplodePower * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Explode()
    {
        Damage damage = new Damage();
        damage.Value = ExplodeDamage;
        damage.From = this.gameObject;
        damage.KnockValue = 0.2f;

        // IDamageable로 바꾸기
        Collider[] colls = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (Collider col in colls)
        {
            if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
        if (Exploding) return;

        CurHealth -= damage.Value;
        if (CurHealth <= 0)
        {
            Debug.Log("barrel exploded boom");
            Flying = StartCoroutine(FlyAway(damage.KnockDir, damage.KnockValue));
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

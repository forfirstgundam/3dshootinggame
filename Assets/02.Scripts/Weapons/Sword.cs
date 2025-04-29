using UnityEngine;
using System.Collections;

public class Sword : WeaponBase
{
    public WeaponStatsSO Stat;
    private float _timer;

    private void Start()
    {
        Player.Instance.CurrentAnimator = GetComponent<Animator>();
        _timer = 0f;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    public override void Attack()
    {
        if(_timer <= 0)
        {
            Swing();
        }
    }

    private void Swing()
    {
        // 원형 범위 내 대상을 검출한다.
        Collider[] cols = Physics.OverlapSphere(Player.Instance.transform.position, Stat.SwingRange);
        Damage damage = new Damage();
        damage.Value = Stat.DamageValue;
        damage.KnockValue = Stat.KnockValue;
        damage.From = Player.Instance.gameObject;
        Player.Instance.CurrentAnimator.SetTrigger("Attack");

        foreach (var col in cols)
        {
            // 검출한 대상의 방향을 구한다.
            Vector3 direction = (col.transform.position - transform.position).normalized;

            if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if (Vector3.Angle(transform.forward, direction) < (Stat.SwingAngle / 2))
                {
                    if (col.gameObject == Player.Instance) continue;
                    damage.KnockDir = direction;
                    damageable.TakeDamage(damage);
                }
            }
        }

        _timer = Stat.SwingCoolTime;
    }

    public override void OnEquip() => gameObject.SetActive(true);
    public override void OnUnequip() => gameObject.SetActive(false);
}

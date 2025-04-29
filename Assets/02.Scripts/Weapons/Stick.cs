using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    public int SwingDamage = 20;
    public float SwingRange = 2f;
    public float SwingAngle = 100f;

    public int LayerMask;
    public Coroutine SwingCoroutine;
    float initialY;

    public Player Player;

    private void Start()
    {
        PlayerAttack attack = Player.GetComponent<PlayerAttack>();
        initialY = transform.localEulerAngles.y;
        attack.OnSwing += Swing;
        LayerMask = ~(1 << 6);
    }

    private IEnumerator SwingRotate()
    {
        float duration = 0.3f; // �ֵθ��� �� �ð�
        float elapsedTime = 0f;
        float swingAmount = -120f; // y������ -120�� ȸ��
        float targetY = initialY + swingAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 0 ~ 1 ���� t�� �������� ȸ�� ����
            float currentY = Mathf.Lerp(initialY, targetY, t);
            Vector3 currentEuler = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(currentEuler.x, currentY, currentEuler.z);

            yield return null;
        }

        // �ֵθ��� ������ ���� �������� �ǵ���
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, initialY, transform.localEulerAngles.z);
    }

    private void Swing()
    {
        // ���� ���� �� ����� �����Ѵ�.
        Collider[] cols = Physics.OverlapSphere(Player.transform.position, SwingRange, LayerMask);
        Damage damage = new Damage();
        damage.Value = SwingDamage;
        damage.KnockValue = 4f;
        damage.From = Player.gameObject;
        if(SwingCoroutine == null)
        {
            SwingCoroutine = StartCoroutine(SwingRotate());
        }
        else
        {
            StopCoroutine(SwingCoroutine);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, initialY, transform.localEulerAngles.z);
            SwingCoroutine = StartCoroutine(SwingRotate());
        }

            foreach (var col in cols)
            {
                // ������ ����� ������ ���Ѵ�.
                Vector3 direction = (col.transform.position - transform.position).normalized;

                if (col.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    if (Vector3.Angle(transform.forward, direction) < (SwingAngle / 2))
                    {
                        print("target in angle");
                        damage.KnockDir = direction;
                        damageable.TakeDamage(damage);
                    }
                }
            }
    }
}

using UnityEngine;

public class Bomb : MonoBehaviour
{
    /* ��ǥ : ���콺�� ������ ��ư : �ٶ󺸴� �������� ����ź
     * 1. ����ź ������Ʈ �����
     * 2. ������ ��ư �Է� �ޱ�
     * 3. �߻� ��ġ�� ����ź ����
     * ������ ����ź�� ī�޶� �������� �� ���ϱ�
     */
    public GameObject ExplosionEffectPrefab;

    // �浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        Destroy(gameObject);
    }
}

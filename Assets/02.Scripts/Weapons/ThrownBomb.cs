using UnityEngine;

public class ThrownBomb : MonoBehaviour
{
    public GameObject ExplosionEffectPrefab;

    // �浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        CameraEffect.Instance.ShakeCamera(0.3f, 2f);

        gameObject.SetActive(false);
    }
}

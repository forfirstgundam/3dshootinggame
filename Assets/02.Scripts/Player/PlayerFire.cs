using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public PlayerStatsSO Stat;

    public GameObject FirePosition;
    public GameObject BombPrefab;
    public float ThrowPower = 15f;

    public int BombCount;

    public ParticleSystem BulletEffect;

    private void FireBullets()
    {
        // Ray :  ������(���� ��ġ, ����)
        // RayCast : �������� �߻�
        // RayCastHit: �������� �ε��� ��ü ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);

            if (isHit)
            {
                // Hit effect
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal;
                BulletEffect.Play();
                Debug.Log("fire bullets");
            }
            
        }
    }

    private void FireBomb()
    {
        if(BombCount > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                GameObject bomb = Instantiate(BombPrefab);
                bomb.transform.position = FirePosition.transform.position;

                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

                bombRigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
                bombRigidbody.AddTorque(Vector3.one);
                BombCount--;
                Debug.Log(BombCount);
            }
        }
    }

    private void Awake()
    {
        BombCount = Stat.MaxBomb;
    }

    private void Update()
    {
        FireBullets();
        FireBomb();
    }
}

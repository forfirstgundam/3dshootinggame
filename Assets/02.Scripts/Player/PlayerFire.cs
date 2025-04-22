using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public PlayerStatsSO Stat;

    public GameObject FirePosition;
    public GameObject BombPrefab;

    private float _curThrowPower;

    public int BombCount;

    public ParticleSystem BulletEffect;

    private void FireBullets()
    {
        // Ray :  레이저(시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RayCastHit: 레이저가 부딪힌 물체 저장
        if (Input.GetMouseButtonUp(0))
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
            if (Input.GetMouseButton(1))
            {
                _curThrowPower += Time.deltaTime * 10f;
                _curThrowPower = Mathf.Min(Stat.MaxThrowPower, _curThrowPower);
                Debug.Log($"throw power is {_curThrowPower}");
            }

            if (Input.GetMouseButtonUp(1))
            {
                GameObject bomb = Instantiate(BombPrefab);
                bomb.transform.position = FirePosition.transform.position;

                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

                bombRigidbody.AddForce(Camera.main.transform.forward * Stat.MinThrowPower, ForceMode.Impulse);
                bombRigidbody.AddTorque(Vector3.one);
                BombCount--;
                UIManager.Instance.UpdateBombNum(BombCount);
                Debug.Log(BombCount);
                _curThrowPower = Stat.MinThrowPower;
            }
        }
    }

    private void Start()
    {
        BombCount = Stat.MaxBomb;
        _curThrowPower = Stat.MinThrowPower;
        UIManager.Instance.UpdateBombNum(BombCount);
    }

    private void Update()
    {
        FireBullets();
        FireBomb();
    }
}

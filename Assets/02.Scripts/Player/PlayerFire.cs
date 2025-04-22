using UnityEngine;
using System;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    public PlayerStatsSO Stat;

    public GameObject FirePosition;
    public GameObject BombPrefab;

    private float _curThrowPower;
    private float _bulletTimer;

    private int _curBullet;
    public int BombCount;

    public ParticleSystem BulletEffect;

    private Coroutine _loadBullet;

    private IEnumerator LoadBullet()
    {
        Debug.Log("loading bullet");
        float timer = 0f;
        UIManager.Instance.ShowLoadBar();

        while(timer <= Stat.LoadTime)
        {
            timer += Time.deltaTime;
            UIManager.Instance.LoadBarUpdate(timer);
            yield return null;
        }
        _curBullet = Stat.MaxBullet;
        UIManager.Instance.UpdateBulletNum(_curBullet);
        UIManager.Instance.HideLoadBar();
    }

    private void FireBullets()
    {
        if (Input.GetMouseButton(0))
        {
            if (_bulletTimer <= 0f && _curBullet > 0)
            {
                if(_loadBullet != null)
                {
                    StopCoroutine(_loadBullet);
                    _loadBullet = null;
                    Debug.Log("stop loading");
                    UIManager.Instance.HideLoadBar();
                }
                InstantiateBullets();
            }
        }
    }

    private void InstantiateBullets()
    {
        // Ray :  레이저(시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RayCastHit: 레이저가 부딪힌 물체 저장
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();

        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            // Hit effect
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal;
            BulletEffect.Play();
        }
        _curBullet--;
        UIManager.Instance.UpdateBulletNum(_curBullet);
        Debug.Log($"left bullets : {_curBullet}");
        _bulletTimer = Stat.BulletCoolTime;
        CameraVibrate.Instance.ShakeCamera();
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
                GameObject bomb = Pools.Instance.Create(0, FirePosition.transform.position);

                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

                bombRigidbody.AddForce(Camera.main.transform.forward * _curThrowPower, ForceMode.Impulse);
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
        _curBullet = Stat.MaxBullet;

        _bulletTimer = 0f;

        UIManager.Instance.UpdateBombNum(BombCount);
        UIManager.Instance.UpdateBulletNum(_curBullet);
    }

    private void Update()
    {
        _bulletTimer -= Time.deltaTime;
        if(Input.GetKey(KeyCode.R) && _loadBullet == null)
        {
            _loadBullet = StartCoroutine(LoadBullet());
        }
        FireBullets();
        FireBomb();
    }
}

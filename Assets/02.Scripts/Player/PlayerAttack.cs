using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using System;

public enum Weapon
{
    Gun,
    Sword,
    Bomb,
}

public class PlayerAttack : MonoBehaviour
{
    public PlayerStatsSO Stat;
    public Weapon CurrentWeapon = Weapon.Gun;

    public GameObject FirePosition;
    public GameObject BombPrefab;
    public GameObject Gun;
    public GameObject Sword;
    private Animator _animator;

    private float _curThrowPower;
    private float _bulletTimer;

    private int _curBullet;
    public int BombCount;

    public ParticleSystem BulletEffect;

    private Coroutine _loadBullet;
    public Action OnSwing;

    public GameObject UI_SniperZoom;
    private bool _zoomMode = false;
    public int ZoomInSize = 15;
    public int ZoomOutSize = 60;

    [SerializeField] private LineRenderer bulletLinePrefab;
    [SerializeField] private float lineDuration = 0.05f;

    private IEnumerator LoadBullet()
    {
        Debug.Log("loading bullet");
        float timer = 0f;
        MainUI.Instance.ShowLoadBar();

        while(timer <= Stat.LoadTime)
        {
            timer += Time.deltaTime;
            MainUI.Instance.LoadBarUpdate(timer);
            yield return null;
        }
        _curBullet = Stat.MaxBullet;
        MainUI.Instance.UpdateBulletNum(_curBullet);
        MainUI.Instance.HideLoadBar();
    }

    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("switched to gun");
            Sword.SetActive(false);
            CurrentWeapon = Weapon.Gun;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("switched to sword");
            Sword.SetActive(true);
            CurrentWeapon = Weapon.Sword;
        }
    }

    private void FireBullets()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                if (_bulletTimer <= 0f && _curBullet > 0)
                {
                    if (_loadBullet != null)
                    {
                        StopCoroutine(_loadBullet);
                        _loadBullet = null;
                        Debug.Log("stop loading");
                        MainUI.Instance.HideLoadBar();
                    }
                    _animator.SetTrigger("Attack");
                    InstantiateBullets();
                }
            }
        }
        else
        {
            Debug.Log("is on ui??");
        }
    }

    private void InstantiateBullets()
    {
        // Ray :  레이저(시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RayCastHit: 레이저가 부딪힌 물체 저장
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        Debug.DrawRay(FirePosition.transform.position, Camera.main.transform.forward * 100f, Color.red, 1f);


        bool isHit = Physics.Raycast(ray, out hitInfo);

        if (isHit)
        {
            DrawLine(FirePosition.transform.position, hitInfo.point);
            // Hit effect
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal;
            BulletEffect.Play();

            if( hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Damage damage = new Damage();
                damage.Value = 20;
                damage.From = gameObject;
                damage.KnockValue = 0.5f;
                damage.KnockDir = hitInfo.point - FirePosition.transform.position;

                damageable.TakeDamage(damage);
            }
        }
        _curBullet--;
        MainUI.Instance.UpdateBulletNum(_curBullet);
        _bulletTimer = Stat.BulletCoolTime;
        CameraEffect.Instance.ShakeCamera();
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
                MainUI.Instance.UpdateBombNum(BombCount);
                Debug.Log(BombCount);
                _curThrowPower = Stat.MinThrowPower;
            }
            _animator.SetTrigger("ThrowBomb");
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        LineRenderer line = Instantiate(bulletLinePrefab);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        StartCoroutine(DisableLineAfter(line, lineDuration));
    }

    private IEnumerator DisableLineAfter(LineRenderer line, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(line.gameObject); // 또는 풀링 시스템 사용 시 비활성화
    }
    private void Start()
    {
        BombCount = Stat.MaxBomb;
        _curThrowPower = Stat.MinThrowPower;
        _curBullet = Stat.MaxBullet;
        _animator = GetComponentInChildren<Animator>();

        _bulletTimer = 0f;

        MainUI.Instance.UpdateBombNum(BombCount);
        MainUI.Instance.UpdateBulletNum(_curBullet);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        _bulletTimer -= Time.deltaTime;
        WeaponSwitch();
        if(CurrentWeapon == Weapon.Gun) {
            if (Input.GetKey(KeyCode.R) && _loadBullet == null)
            {
                _loadBullet = StartCoroutine(LoadBullet());
            }

            if (Input.GetMouseButtonDown(1))
            {
                _zoomMode = !_zoomMode;

                if (_zoomMode)
                {
                    UI_SniperZoom.SetActive(true);
                    Camera.main.fieldOfView = ZoomInSize;
                }
                else
                {
                    UI_SniperZoom.SetActive(false);
                    Camera.main.fieldOfView = ZoomOutSize;
                }
            }

            FireBullets();
        } else if(CurrentWeapon == Weapon.Sword)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnSwing?.Invoke();
            }
        }
        
        FireBomb();
    }
}

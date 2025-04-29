using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Gun : WeaponBase
{
    public WeaponStatsSO Stat;

    public GameObject FirePosition;
    public GameObject GunModel;
    public ParticleSystem BulletEffect;

    private float _bulletTimer;
    private int _curBullet;
    private Coroutine _loadBullet;

    public GameObject UI_SniperZoom;
    private bool _zoomMode = false;
    public int ZoomInSize = 15;
    public int ZoomOutSize = 60;

    [SerializeField] private LineRenderer bulletLinePrefab;
    [SerializeField] private float lineDuration = 0.05f;

    private void OnEnable()
    {
        Player.Instance.CurrentAnimator = GetComponent<Animator>();
        _bulletTimer = 0f;
        _curBullet = Stat.MaxBullet;

        MainUI.Instance.UpdateBulletNum(Stat.MaxBullet);
    }

    private void Update()
    {
        _bulletTimer -= Time.deltaTime;
    }

    public override void Attack()
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
                    Player.Instance.CurrentAnimator.SetTrigger("Attack");
                    InstantiateBullets();
                }
            }
        }
    }

    public void LoadGun()
    {
        if(_loadBullet == null)
        {
            _loadBullet = StartCoroutine(LoadBullet());
        }
    }

    public void ToggleSniperMode()
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

    private IEnumerator LoadBullet()
    {
        Debug.Log("loading bullet");
        float timer = 0f;
        MainUI.Instance.ShowLoadBar();

        while (timer <= Stat.LoadTime)
        {
            timer += Time.deltaTime;
            MainUI.Instance.LoadBarUpdate(timer);
            yield return null;
        }
        _curBullet = Stat.MaxBullet;
        MainUI.Instance.UpdateBulletNum(_curBullet);
        MainUI.Instance.HideLoadBar();
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

            if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Damage damage = new Damage();
                damage.Value = Stat.DamageValue;
                damage.From = gameObject;
                damage.KnockValue = Stat.KnockValue;
                damage.KnockDir = hitInfo.point - FirePosition.transform.position;

                damageable.TakeDamage(damage);
            }
        }
        _curBullet--;
        MainUI.Instance.UpdateBulletNum(_curBullet);
        _bulletTimer = Stat.BulletCoolTime;
        CameraEffect.Instance.ShakeCamera();
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

    public override void OnEquip() => gameObject.SetActive(true);
    public override void OnUnequip() => gameObject.SetActive(false);
}

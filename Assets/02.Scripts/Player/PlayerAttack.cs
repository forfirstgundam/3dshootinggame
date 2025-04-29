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
    public Player Player;

    public Weapon CurrentWeapon = Weapon.Gun;

    public WeaponBase[] Weapons; // 0: Gun, 1: Sword, 2: Bomb
    private int _currentWeaponIndex = 0;
    private WeaponBase _currentWeapon;

    private void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            EquipWeapon(2);
        else if (Input.mouseScrollDelta.y != 0) // 마우스 휠
        {
            int dir = (int)Mathf.Sign(Input.mouseScrollDelta.y);
            int newIndex = (_currentWeaponIndex - dir + Weapons.Length) % Weapons.Length;
            EquipWeapon(newIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        if (_currentWeapon != null) _currentWeapon.OnUnequip();

        _currentWeaponIndex = index;
        _currentWeapon = Weapons[_currentWeaponIndex];
        _currentWeapon.OnEquip();
    }

    private void Start()
    {
        EquipWeapon(_currentWeaponIndex);
        Player = GetComponent<Player>();

        MainUI.Instance.UpdateBombNum(3);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
        HandleWeaponSwitchInput();
        if (Input.GetMouseButtonDown(0))
        {
            _currentWeapon.Attack();
        }
        if(CurrentWeapon == Weapon.Gun)
        {
            Gun gun = _currentWeapon.gameObject.GetComponent<Gun>();
            if (Input.GetKeyDown(KeyCode.R))
            {
                gun.LoadGun();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                gun.ToggleSniperMode();
            }
        }
        /*if(CurrentWeapon == Weapon.Gun) {
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
        }*/
    }
}

/*private IEnumerator LoadBullet()
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

private void WeaponSwitch()
{
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        Debug.Log("switched to gun");
        Sword.SetActive(false);
        CurrentWeapon = Weapon.Gun;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
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
                CurrentAnimator.SetTrigger("Attack");
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

        if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
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
}*/
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
        else if (Input.mouseScrollDelta.y != 0) // ¸¶¿ì½º ÈÙ
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
        CurrentWeapon = (Weapon)_currentWeaponIndex;
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
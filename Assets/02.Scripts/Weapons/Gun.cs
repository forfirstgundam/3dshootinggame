/*using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Gun : WeaponBase
{
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

    public override void OnEquip() => gameObject.SetActive(true);
    public override void OnUnequip() => gameObject.SetActive(false);
}
*/
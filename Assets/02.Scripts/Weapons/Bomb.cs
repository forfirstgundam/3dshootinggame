using UnityEngine;

public class Bomb : WeaponBase
{
    /* 목표 : 마우스의 오른쪽 버튼 : 바라보는 방향으로 수류탄
     * 1. 수류탄 오브젝트 만들기
     * 2. 오른쪽 버튼 입력 받기
     * 3. 발사 위치에 수류탄 생성
     * 생성된 수류탄을 카메라 방향으로 힘 가하기
     */
    public WeaponStatsSO Stat;
    public GameObject FirePosition;
    private float _curThrowPower;

    public int BombCount;

    private void Start()
    {
        BombCount = Stat.MaxBomb;
        MainUI.Instance.UpdateBombNum(BombCount);

        _curThrowPower = Stat.MinThrowPower;
    }

    private void OnEnable()
    {
        Player.Instance.CurrentAnimator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            _curThrowPower += Time.deltaTime * 10f;
            _curThrowPower = Mathf.Min(Stat.MaxThrowPower, _curThrowPower);
            Debug.Log($"throw power is {_curThrowPower}");
        }

        if (Input.GetMouseButtonUp(0))
        {
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        GameObject bomb = Pools.Instance.Create(0, FirePosition.transform.position);

        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();

        bombRigidbody.AddForce(Camera.main.transform.forward * _curThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
        BombCount--;
        MainUI.Instance.UpdateBombNum(BombCount);
        Debug.Log(BombCount);
        _curThrowPower = Stat.MinThrowPower;

        Player.Instance.CurrentAnimator.SetTrigger("Throw");
    }

    public override void OnEquip()
    {

    }
    public override void OnUnequip()
    {

    }
}
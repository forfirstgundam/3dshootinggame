using UnityEngine;

public class Bomb : WeaponBase
{
    /* ��ǥ : ���콺�� ������ ��ư : �ٶ󺸴� �������� ����ź
     * 1. ����ź ������Ʈ �����
     * 2. ������ ��ư �Է� �ޱ�
     * 3. �߻� ��ġ�� ����ź ����
     * ������ ����ź�� ī�޶� �������� �� ���ϱ�
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
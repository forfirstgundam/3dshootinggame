using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class Coin : MonoBehaviour
{
    public float SpinSpeed = 20f;
    public float HoverOffset = 0.08f;
    public float RaycastDistance = 5f;
    public float PlayerPullDistance = 1f;
    public float PlayerEatDistance = 0.1f;

    private bool _isPulled = false;

    private void GoToPlayer()
    {
        transform.DOMove(Player.Instance.transform.position, 0.4f)
        .SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            CoinAbsorb();
        });
    }

    private void CoinAbsorb()
    {
        Debug.Log("coin will go to player");
        MainUI.Instance.UpdateCoinNum(1);
        gameObject.SetActive(false);
    }

    // spin while existing
    void Update()
    {
        transform.Rotate(Vector3.left, SpinSpeed * Time.deltaTime);

        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (!_isPulled && distanceToPlayer <= PlayerPullDistance)
        {
            _isPulled = true;
            GoToPlayer();
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PlayerStatsSO Stats;
    public Slider StaminaBar;
    public Slider LoadBulletBar;
    public TextMeshProUGUI BombNumber;
    public TextMeshProUGUI BulletNumber;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBombNum(int num)
    {
        BombNumber.text = $"��ź : {num} / {Stats.MaxBomb}";
    }

    public void UpdateBulletNum(int num)
    {
        BulletNumber.text = $"�Ѿ� : {num} / {Stats.MaxBullet}";
    }

    public void ShowLoadBar()
    {
        LoadBulletBar.gameObject.SetActive(true);
    }
    public void HideLoadBar()
    {
        LoadBulletBar.gameObject.SetActive(false);
    }

    public void LoadBarUpdate(float progress)
    {
        LoadBulletBar.value = (progress / Stats.LoadTime);
    }

    private void Update()
    {
        StaminaBar.value = (Stats.Stamina / Stats.MaxStamina);
        //LoadBulletBar.value
    }
}

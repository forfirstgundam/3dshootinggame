using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance;
    public PlayerStatsSO Stats;

    public Slider StaminaBar;
    public Slider LoadBulletBar;

    public TextMeshProUGUI BombNumber;
    public TextMeshProUGUI BulletNumber;

    public Button MinimapPlus;
    public Button MinimapMinus;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBombNum(int num)
    {
        BombNumber.text = $"ÆøÅº : {num} / {Stats.MaxBomb}";
    }

    public void UpdateBulletNum(int num)
    {
        BulletNumber.text = $"ÃÑ¾Ë : {num} / {Stats.MaxBullet}";
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

    public void OnMapPlus()
    {
        MinimapCamera.Instance.MinimapScaleChange(true);
    }

    public void OnMapMinus()
    {
        MinimapCamera.Instance.MinimapScaleChange(false);
    }

    private void Update()
    {
        StaminaBar.value = (Stats.Stamina / Stats.MaxStamina);
        //LoadBulletBar.value
    }
}

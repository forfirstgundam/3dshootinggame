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

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBombNum(int num)
    {
        BombNumber.text = $"ÆøÅº : {num} / {Stats.MaxBomb}";
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

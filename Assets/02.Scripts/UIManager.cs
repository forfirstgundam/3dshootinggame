using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PlayerStatsSO Stats;
    public Slider StaminaBar;
    public TextMeshProUGUI BombNumber;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBombNum(int num)
    {
        BombNumber.text = $"ÆøÅº : {num} / {Stats.MaxBomb}";
    }

    private void Update()
    {
        StaminaBar.value = (Stats.Stamina / Stats.MaxStamina);
    }
}

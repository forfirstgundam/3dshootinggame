using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    public PlayerStatsSO Stats;
    public Slider StaminaBar;

    private void Update()
    {
        StaminaBar.value = (Stats.Stamina / Stats.MaxStamina);
    }
}

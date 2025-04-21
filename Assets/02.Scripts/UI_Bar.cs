using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    public Slider StaminaBar;

    private void Update()
    {
        StaminaBar.value = (PlayerStamina.Stamina / PlayerStamina.MaxStamina);
    }
}

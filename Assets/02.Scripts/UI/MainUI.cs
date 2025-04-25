using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance;
    public PlayerStatsSO Stats;

    public Slider StaminaBar;
    public Slider LoadBulletBar;

    public TextMeshProUGUI BombNumber;
    public TextMeshProUGUI BulletNumber;

    public Image GlitchEffect;
    public Coroutine GlitchOn;

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

    public void GlitchEffectOn()
    {
        if(GlitchOn == null)
        {
            GlitchOn = StartCoroutine(Glitching());
        } else
        {
            StopCoroutine(GlitchOn);
            GlitchOn = StartCoroutine(Glitching());
        }
    }

    private IEnumerator Glitching()
    {
        
        yield return null;
    }

    private void Update()
    {
        StaminaBar.value = (Stats.Stamina / Stats.MaxStamina);
        //LoadBulletBar.value

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            OnMapMinus();
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            OnMapPlus();
        }
    }
}

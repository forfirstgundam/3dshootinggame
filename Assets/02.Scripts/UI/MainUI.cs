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
        RectTransform rect = GlitchEffect.rectTransform;
        Vector3 originalPos = rect.localPosition;

        float duration = 0.5f;
        float elapsed = 0f;
        float maxShake = 200f;

        Color color = GlitchEffect.color;
        color.a = 1f;
        GlitchEffect.color = color;
        GlitchEffect.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            // ���� ����
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            color.a = alpha;
            GlitchEffect.color = color;

            // ���� ��鸲
            float offsetX = Random.Range(-maxShake, maxShake);
            float offsetY = Random.Range(-maxShake, maxShake);
            rect.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }

        // ������
        color.a = 0f;
        GlitchEffect.color = color;
        rect.localPosition = originalPos;
        GlitchEffect.gameObject.SetActive(false);
        GlitchOn = null;
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

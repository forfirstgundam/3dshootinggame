using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance;
    public PlayerStatsSO Stats;
    public WeaponStatsSO WeaponStats;

    public Slider StaminaBar;
    public Slider LoadBulletBar;
    public Slider HealthBar;

    public GameObject[] WeaponIcons;

    public TextMeshProUGUI CoinNumber;
    public TextMeshProUGUI BombNumber;
    public TextMeshProUGUI BulletNumber;

    public Image GlitchEffect;
    public Coroutine GlitchOn;

    public Button MinimapPlus;
    public Button MinimapMinus;

    // 나중에 currency 등으로 분리
    public int CoinAmount;

    private void Awake()
    {
        Instance = this;
        CoinAmount = 0;
    }

    private void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        MainUI.Instance.UpdateCoinNum(0);
        player.OnPlayerHit += UpdateHealthBar;
    }
    public void UpdateCoinNum(int num)
    {
        CoinAmount += num;
        CoinNumber.text = $"코인 : {CoinAmount}";
        CoinNumber.text = $"코인 : {CoinAmount}";
    }

    public void UpdateBombNum(int num)
    {
        BombNumber.text = $"폭탄 : {num} / {WeaponStats.MaxBomb}";
    }

    public void UpdateBulletNum(int num)
    {
        BulletNumber.text = $"총알 : {num} / {WeaponStats.MaxBullet}";
    }

    public void ShowLoadBar()
    {
        LoadBulletBar.gameObject.SetActive(true);
    }
    public void HideLoadBar()
    {
        LoadBulletBar.gameObject.SetActive(false);
    }

    public void UpdateHealthBar(int hp)
    {
        Debug.Log($"health is {hp}");
        HealthBar.value = (float)hp / (float)Stats.MaxHealth;
    }

    public void LoadBarUpdate(float progress)
    {
        LoadBulletBar.value = (progress / WeaponStats.LoadTime);
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

    public void SwitchWeaponIcon(int index)
    {
        foreach(GameObject icon in WeaponIcons)
        {
            icon.SetActive(false);
        }

        WeaponIcons[index].SetActive(true);
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
            // 알파 감소
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            color.a = alpha;
            GlitchEffect.color = color;

            // 랜덤 흔들림
            float offsetX = Random.Range(-maxShake, maxShake);
            float offsetY = Random.Range(-maxShake, maxShake);
            rect.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }

        // 마무리
        color.a = 0f;
        GlitchEffect.color = color;
        rect.localPosition = originalPos;
        GlitchEffect.gameObject.SetActive(false);
        GlitchOn = null;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Play) return;
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

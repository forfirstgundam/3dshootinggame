using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Ready,
    Play,
    GameOver,
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Coroutine ReadyState;
    public float ReadyTime = 2f;

    public float GoTime = 0.5f;
    public GameState GameState = GameState.Ready;

    public Image ReadyImage;
    public Slider ReadySlider;
    public TextMeshProUGUI PlayText;
    public GameObject GameOverUI;
    public GameObject PopupUI;

    private void Awake()
    {
        Instance = this;
        ReadyState = StartCoroutine(Ready());
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnPlayerDeath += CallGameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public IEnumerator Ready()
    {
        Debug.Log("Gamemode is now Ready");
        float timer = 0f;
        while(timer<= ReadyTime)
        {
            timer += Time.deltaTime;
            ReadySlider.value = timer / ReadyTime;
            yield return null;
        }

        ReadyImage.gameObject.SetActive(false);
        yield return Play();
    }

    public IEnumerator Play()
    {
        Debug.Log("Gamemode is now Play");
        float timer = 0f;
        GameState = GameState.Play;
        while (timer <= GoTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayText.gameObject.SetActive(false);
    }

    public IEnumerator GameOver()
    {
        Debug.Log("Gamemode is now GameOver");
        GameState = GameState.GameOver;
        GameOverUI.SetActive(true);
        yield return null;
    }

    public void CallGameOver()
    {
        StartCoroutine(GameOver());
    }

    public void Pause()
    {
        // 1. 게임 상태를 Pause로 바꿈
        if(GameState != GameState.Pause)
        {
            GameState = GameState.Pause;
            Time.timeScale = 0f;
            PopupUI.SetActive(true);
        }
        else if (GameState == GameState.Pause)
        {
            GameState = GameState.Play;
            Time.timeScale = 1f;
            PopupUI.SetActive(false);
        }
    }
}

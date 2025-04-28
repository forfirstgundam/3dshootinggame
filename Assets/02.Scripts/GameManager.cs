using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Ready,
    Play,
    GameOver
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

    private void Awake()
    {
        Instance = this;
        ReadyState = StartCoroutine(Ready());
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnPlayerDeath += CallGameOver;
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
}

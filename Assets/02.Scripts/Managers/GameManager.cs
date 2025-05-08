using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("UI List")]
    public GameObject GameOverUI;
    public GameObject PopupUI;
    public GameObject CreditPopUI;

    private LinkedList<GameObject> _popups = new LinkedList<GameObject>();

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
            if(_popups.Count == 0)
            {
                Pause();
            }
            else
            {
                if(_popups.Count > 1)
                {
                    GameObject last = _popups.Last();
                    last.SetActive(false);
                    _popups.RemoveLast();
                }
                else
                {
                    UnPause();
                }
            }
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
        GameState = GameState.Pause;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        _popups.AddLast(PopupUI);

        PopupUI.SetActive(true);
    }

    public void UnPause()
    {
        GameState = GameState.Play;
        Time.timeScale = 1f;
        _popups.RemoveLast();
        PopupUI.SetActive(false);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void Credit()
    {
        Cursor.lockState = CursorLockMode.None;
        _popups.AddLast(CreditPopUI);
        CreditPopUI.SetActive(true);
    }

    public void CloseFront()
    {
        GameObject last = _popups.Last();
        last.SetActive(false);
        _popups.RemoveLast();
    }
}

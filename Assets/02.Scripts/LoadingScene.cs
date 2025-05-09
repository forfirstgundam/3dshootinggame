using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    /* 목표 : 다음 씬을 '비동기 방식'으로 로드
     * 또한 로딩 진행률 시각적 표현
     *                      ㄴ %프로그레스 바, 플레이버 텍스트
     */

    public int NextSceneIndex = 2;

    public Slider ProgressSlider;
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        //SceneManager.LoadScene(NextSceneIndex);
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false;

        while(ao.isDone == false)
        {
            // 이 부분에서 유저 데이터를 받아오면 됨
            Debug.Log(ao.progress);
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100}%";

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

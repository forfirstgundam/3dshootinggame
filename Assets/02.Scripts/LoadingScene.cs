using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    /* ��ǥ : ���� ���� '�񵿱� ���'���� �ε�
     * ���� �ε� ����� �ð��� ǥ��
     *                      �� %���α׷��� ��, �÷��̹� �ؽ�Ʈ
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
            // �� �κп��� ���� �����͸� �޾ƿ��� ��
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

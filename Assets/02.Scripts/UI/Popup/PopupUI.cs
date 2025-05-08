using System.Linq;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public void OnClickContinue()
    {
        GameManager.Instance.UnPause();
    }

    public void OnClickRetry()
    {
        // Retry
        GameManager.Instance.Retry();
    }

    public void OnClickQuit()
    {
        // Quit
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void OnClickCredit()
    {
        // Credit
        GameManager.Instance.Credit();
    }

    public void CreditClose()
    {
        GameManager.Instance.CloseFront();
    }
}

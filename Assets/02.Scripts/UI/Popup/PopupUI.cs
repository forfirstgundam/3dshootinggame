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
    }

    public void OnClickCredit()
    {
        // Credit
        GameManager.Instance.Credit();
    }
}

using System.Collections.Generic;
using UnityEngine;

public enum PopUpType
{
    GameOver,
    PauseMenu,
    Credit,
}

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance;
    public GameObject[] PopUps;

    private List<GameObject> OpenPopUps;
    
    private void Awake()
    {
        Instance = this;
        OpenPopUps = new List<GameObject>();
    }
    
    public void Open(PopUpType type)
    {

    }

    public void Close(PopUpType type)
    {

    }
}

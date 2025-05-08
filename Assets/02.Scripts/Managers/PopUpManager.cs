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

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

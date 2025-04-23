using TMPro;
using UnityEngine;

public class BacklogUI : MonoBehaviour
{
    public static BacklogUI Instance;
    public TextMeshProUGUI BacklogPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Backlog(string str)
    {
        TextMeshProUGUI temp = Instantiate(BacklogPrefab);
        temp.text = str;
        temp.transform.SetParent(this.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Backlog("테스트용입니다");
    }
}
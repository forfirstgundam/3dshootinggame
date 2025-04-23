using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class BacklogUI : MonoBehaviour
{
    public static BacklogUI Instance;
    public TextMeshProUGUI BacklogPrefab;
    public Transform ContentParent;

    public int MaxLogs = 5;

    private Queue<GameObject> _logs = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddLog(string message)
    {
        // �ʰ��Ǹ� ���� ������ �α� ����
        if (_logs.Count >= MaxLogs)
        {
            GameObject oldLog = _logs.Dequeue();
            Destroy(oldLog);
        }

        // �� �α� ����
        TextMeshProUGUI newLog = Instantiate(BacklogPrefab, ContentParent);
        newLog.text = message;
        _logs.Enqueue(newLog.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddLog("�׽�Ʈ �α��Դϴ�.");
        }
    }
}

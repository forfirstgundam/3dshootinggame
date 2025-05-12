using UnityEngine;
using System.Collections.Generic;

public class Pools : MonoBehaviour
{
    public static Pools Instance;

    [Header("������ ����Ʈ")]
    public List<GameObject> PoolObjects;

    [Header("�� �������� �ʱ� ���� ����")]
    public List<int> PoolSizes;

    private List<List<GameObject>> PoolList;

    private void Awake()
    {
        Instance = this;

        PoolList = new List<List<GameObject>>();

        for (int i = 0; i < PoolObjects.Count; i++)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int j = 0; j < PoolSizes[i]; j++)
            {
                GameObject obj = Instantiate(PoolObjects[i]);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                pool.Add(obj);
            }

            PoolList.Add(pool);
        }
    }

    public GameObject Create(int index, Vector3 position)
    {
        foreach (var obj in PoolList[index])
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }

        // ������ ���� �����ؼ� ����Ʈ�� �߰�
        GameObject newObj = Instantiate(PoolObjects[index], position, Quaternion.identity, transform);
        newObj.SetActive(true);
        PoolList[index].Add(newObj);
        return newObj;
    }
}

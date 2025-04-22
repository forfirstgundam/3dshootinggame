using UnityEngine;
using System.Collections.Generic;

public class Pools : MonoBehaviour
{
    public static Pools Instance;

    [Header("프리팹 리스트")]
    public List<GameObject> PoolObjects;

    [Header("각 프리팹의 초기 생성 개수")]
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

        // 없으면 새로 생성해서 리스트에 추가
        GameObject newObj = Instantiate(PoolObjects[index], position, Quaternion.identity, transform);
        newObj.SetActive(true);
        PoolList[index].Add(newObj);
        return newObj;
    }
}

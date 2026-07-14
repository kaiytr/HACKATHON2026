using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PoolItem
{
    public string poolName;
    public GameObject prefab;
    public int initCount = 10;
}

public class PoolManager : Singleton<PoolManager>
{
    public List<PoolItem> poolItems;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < item.initCount; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.poolName, objectPool);
        }
    }

    // 사용법: GameObject bullet = PoolManager.Instance.SpawnFromPool("Bullet", transform.position, Quaternion.identity);
    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning($"Pool 없음: {poolName}");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[poolName].Dequeue();

        // 만약 풀이 모자라면 자동 확장
        if (objectToSpawn.activeSelf)
        {
            poolDictionary[poolName].Enqueue(objectToSpawn); // 기존 거 다시 넣어두고
            // 새로 하나 복제해서 씀
            objectToSpawn = Instantiate(objectToSpawn, transform);
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[poolName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

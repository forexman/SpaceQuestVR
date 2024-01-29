using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    public static PoolManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string key, GameObject prefab, int size)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        pools[key] = objectPool;
    }

    public GameObject GetFromPool(string key)
    {
        if (!pools.ContainsKey(key) || pools[key].Count == 0) return null;

        GameObject obj = pools[key].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}

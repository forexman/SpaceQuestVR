using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>(); // Track active objects
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

    public void CreatePool(string poolKey, GameObject prefab, int poolSize)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = InstantiatePrefab(poolKey, prefab);
            objectPool.Enqueue(obj);
        }

        pools.Add(poolKey, objectPool);
        prefabLookup.Add(poolKey, prefab);
    }

    private GameObject InstantiatePrefab(string poolKey, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = poolKey;
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    public GameObject GetFromPool(string key)
    {
        GameObject obj;
        if (!pools.ContainsKey(key))
        {
            Debug.LogError("Pool with key " + key + " does not exist.");
            return null;
        }
        else if (pools[key].Count == 0)
        {
            // Dynamically create a new object if the pool is empty
            obj = InstantiatePrefab(key, prefabLookup[key]);
        }
        else
        {
            obj = pools[key].Dequeue();
        }

        obj.SetActive(true);
        activeObjects.Add(obj);
        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError("Pool with key " + key + " does not exist.");
            Destroy(obj);
            return;
        }
        obj.SetActive(false);
        activeObjects.Remove(obj);
        pools[key].Enqueue(obj);
    }

    public void ReturnAllActiveItemsToPool()
    {
        foreach (var obj in new List<GameObject>(activeObjects)) // Using a copy to modify the list safely
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                pools[obj.name].Enqueue(obj);
            }
        }
        activeObjects.Clear();
    }
}

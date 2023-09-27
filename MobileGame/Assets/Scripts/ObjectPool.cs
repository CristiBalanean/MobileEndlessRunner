using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private int amount;

    [SerializeField] private GameObject[] objectsToPool;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, objectsToPool.Length);
            GameObject obj = Instantiate(objectsToPool[rand]);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolHighway : MonoBehaviour
{
    public static ObjectPoolHighway instance;

    private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private int amount;

    [SerializeField] private GameObject objectToPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(objectToPool);
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

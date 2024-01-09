using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private int amount;

    [SerializeField] private GameObject objectToPool;
    [SerializeField] private GameObject overtakeText;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            if (objectToPool == overtakeText)
            {
                obj.transform.SetParent(CarMovement.Instance.transform);
            }
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

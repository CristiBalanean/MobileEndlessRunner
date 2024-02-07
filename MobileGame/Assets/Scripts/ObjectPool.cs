using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    [SerializeField] private int amount;

    [SerializeField] private GameObject objectToPool;
    [SerializeField] private GameObject overtakeText;
    [SerializeField] private Sprite[] mapSprites;

    private void Awake()
    {
        if (objectToPool.name == "Highway")
            objectToPool.GetComponent<SpriteRenderer>().sprite = mapSprites[GameModeData.instance.map];
    }

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
        List<GameObject> inactiveObjects = new List<GameObject>();

        // Collect all inactive objects in a separate list
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                inactiveObjects.Add(pool[i]);
            }
        }

        // Return a random inactive object, if any
        if (inactiveObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveObjects.Count);
            return inactiveObjects[randomIndex];
        }

        return null; // Return null if no inactive objects are found
    }
}

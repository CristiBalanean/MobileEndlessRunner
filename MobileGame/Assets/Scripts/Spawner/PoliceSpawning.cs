using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawning : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject policePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void PoliceEventChance()
    {
        float rand = Random.Range(0, 1000);

        if (rand < 30)
            SpawnPoliceEvent();
    }

    private void SpawnPoliceEvent()
    {
        int randomSpawn = Random.Range(0, spawnPoints.Length);
        Instantiate(policePrefab, spawnPoints[randomSpawn].transform.position, Quaternion.identity);
    }
}

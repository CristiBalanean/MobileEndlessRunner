using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPickupSpawner : MonoBehaviour
{
    [SerializeField] private float gasPickupSpawnTime;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private GameObject gasPickupGO;

    private float currentTime;

    private void Start()
    {
        currentTime = gasPickupSpawnTime;
    }

    private void Update()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
        {
            currentTime = gasPickupSpawnTime;
            SpawnGasPickup();
        }
    }

    private void SpawnGasPickup()
    {
        int rand = Random.Range(0, spawners.Length);
        Instantiate(gasPickupGO, spawners[rand].position, Quaternion.identity);
    }
}

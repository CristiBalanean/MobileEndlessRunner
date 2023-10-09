using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawning : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] powerUps;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PowerUpChance", 30f, 2.5f);
    }

    private void PowerUpChance()
    {
        float rand = Random.Range(0, 1000); // Use a range up to 1000

        if (rand < 10) // 70 represents 7% of 1000
            SpawnPowerUp();
    }

    private void SpawnPowerUp()
    {
        int randomSpawn = Random.Range(0, spawnPoints.Length);
        int randomPowerUp = Random.Range(0, powerUps.Length);
        Instantiate(powerUps[randomPowerUp], spawnPoints[randomSpawn].transform.position, Quaternion.identity);
    }
}

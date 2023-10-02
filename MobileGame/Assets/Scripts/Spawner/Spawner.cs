using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject policePrefab;

    [SerializeField] private float timeToSpawn;
    [SerializeField] private float minTime;
    [SerializeField] private float timeDecreaseRate;
    [SerializeField] private float currentTime;

    // Define the Y-position range for the road curvature
    private float minY = -1.0f;  // Adjust these values as needed
    private float maxY = 1.0f;

    private void Start()
    {
        currentTime = timeToSpawn;

        InvokeRepeating("PowerUpChance", 30f, 10f);
        InvokeRepeating("PoliceEventChance", 30f, 10f);
    }

    private void Update()
    {
        Collider2D[] carNumber = Physics2D.OverlapCircleAll(transform.position, 15f);

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if(carNumber.Length < 5)
            {
                int spawnChoice = Random.Range(0, 2);

                if (spawnChoice == 0)
                {
                    // Spawn one obstacle
                    GameObject obstacle = ObjectPool.instance.GetPooledObject();

                    if (obstacle != null)
                    {
                        int rand = Random.Range(0, spawnPoints.Length);

                        // Generate a random Y-position within the defined range
                        float randomYPosition = Random.Range(minY, maxY);

                        // Assign the Y-position to the obstacle's transform
                        Vector3 obstaclePosition = spawnPoints[rand].transform.position;
                        obstaclePosition.y += randomYPosition;
                        obstacle.transform.position = obstaclePosition;

                        obstacle.SetActive(true);
                        currentTime = timeToSpawn;
                        if (timeToSpawn > minTime)
                            timeToSpawn -= timeDecreaseRate;
                    }
                }
                else
                {
                    int rand1, rand2;
                    do
                    {
                        rand1 = Random.Range(0, spawnPoints.Length);
                        rand2 = Random.Range(0, spawnPoints.Length);
                    } while (rand2 == rand1);

                    GameObject obstacle1 = ObjectPool.instance.GetPooledObject();

                    if (obstacle1 != null)
                    {
                        // Generate random Y-positions for the two obstacles
                        float randomYPosition1 = Random.Range(minY, maxY);
                        float randomYPosition2 = Random.Range(minY, maxY);

                        // Assign Y-positions to the obstacles' transforms
                        Vector3 obstaclePosition1 = spawnPoints[rand1].transform.position;
                        Vector3 obstaclePosition2 = spawnPoints[rand2].transform.position;
                        obstaclePosition1.y += randomYPosition1;
                        obstaclePosition2.y += randomYPosition2;

                        obstacle1.transform.position = obstaclePosition1;
                        obstacle1.SetActive(true);
                    }

                    currentTime = timeToSpawn;

                    if (timeToSpawn > minTime)
                        timeToSpawn -= timeDecreaseRate;
                }
            }
        }
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

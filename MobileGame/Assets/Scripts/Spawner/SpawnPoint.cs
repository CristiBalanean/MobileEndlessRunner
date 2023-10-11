using System.Collections;
using System.Numerics;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float laneSpeed;
    private int laneDensity;
    [SerializeField] private int laneNumber;

    [SerializeField] private Collider2D densityCollider;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float minSpawnDistance = 1.25f; // Adjust the value based on your game's requirements

    [SerializeField] private LayerMask obstacleLayer;

    private void Start()
    {
        float playerTopSpeed = CarMovement.Instance.GetTopSpeed();

        // Adjust the spawn intervals based on the player's top speed using a logarithmic function
        minSpawnInterval = Mathf.Log(playerTopSpeed, 2) / 6 - laneSpeed / 100 + 0.75f;
        maxSpawnInterval = Mathf.Log(playerTopSpeed, 2) / 4 - laneSpeed / 100 + 0.75f;

        InvokeRepeating("CheckLaneDensity", 0f, 1.5f);
        StartCoroutine(StaggeredSpawnRoutine());
    }

    private IEnumerator StaggeredSpawnRoutine()
    {
        while (true)
        {
            // Calculate a random spawn interval for this lane
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Wait for the calculated spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Spawn a vehicle if the lane is not too crowded
            int densityThreshold = GetDynamicDensityThreshold();
            if (laneDensity < densityThreshold) // Adjust the threshold based on dynamic conditions
            {
                SpawnVehicle();
            }
        }
    }

    private int GetDynamicDensityThreshold()
    {
        float playerSpeed = CarMovement.Instance.GetSpeed();
        // Increase density threshold at higher speeds for larger gaps
        if (playerSpeed > 150f)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private void CheckLaneDensity()
    {
        UnityEngine.Vector2 position = densityCollider.transform.position;
        UnityEngine.Vector2 bounds = densityCollider.bounds.size;

        Collider2D[] cars = Physics2D.OverlapBoxAll(position, bounds, 0f);
        laneDensity = cars.Length;
    }

    private void SpawnVehicle()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, minSpawnDistance, obstacleLayer);

        foreach (var collider in nearbyColliders)
        {
            if (collider.CompareTag("Obstacle")) // Check if the collider belongs to a spawned car
            {
                // Car too close, don't spawn a new one
                return;
            }
        }

        GameObject car = ObjectPool.instance.GetPooledObject();
        if (car != null)
        {
            car.transform.position = transform.position;

            // Adjust spawn position based on lane density to avoid overlap
            car.GetComponent<Obstacle>().topSpeed = Random.Range((laneSpeed - 4.5f) / 3.6f, laneSpeed / 3.6f) * CarMovement.Instance.speedMultiplier;
            car.SetActive(true);
        }
    }
}

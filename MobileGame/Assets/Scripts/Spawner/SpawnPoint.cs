using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private Collider2D densityCollider;

    [SerializeField] private int maxLaneDensity;
    [SerializeField] private float laneSpeed;
    [SerializeField] private float spawnTime;
    private float currentTime;

    private void Start()
    {
        currentTime = spawnTime;
    }

    private void Update()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
        {
            SpawnVehicle();
        }
    }

    private void SpawnVehicle()
    {
        if (CheckIfCanSpawn() && CheckLaneDensity() <= maxLaneDensity)
        {
            GameObject car = ObjectPool.instance.GetPooledObject();
            if(car != null)
            {
                car.transform.position = transform.position;
                car.GetComponent<Obstacle>().topSpeed = (Random.Range(50, 60) + CarMovement.Instance.speedMultiplier) / 3.6f;
                car.SetActive(true);
                currentTime = spawnTime;
            }
        }
    }

    private bool CheckIfCanSpawn()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 15f, obstacleLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 15f, obstacleLayer);

        if (hitUp.transform == null && hitDown.transform == null)
            return true;
        else return false;
    }

    private int CheckLaneDensity()
    {
        UnityEngine.Vector2 position = densityCollider.transform.position;
        UnityEngine.Vector2 bounds = densityCollider.bounds.size;

        // OverlapBoxAll returns all colliders within the specified box
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, bounds, 0f);

        // Filter colliders by tag and count them
        int obstacleCount = 0;
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Obstacle>() != null)
            {
                obstacleCount++;
            }
        }

        return obstacleCount;
    }
}

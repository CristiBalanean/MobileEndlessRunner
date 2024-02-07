using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float offset;

    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

    private void Start()
    {
        InvokeRepeating("ChanceToSpawn", 10f, 5f);
    }

    private void ChanceToSpawn()
    {
        int chance = Random.Range(0, 100);
        if (chance < 40)
        {
            SpawnCoins();
            Debug.Log("Spawned Coins");
        }
    }

    private void SpawnCoins()
    {
        int numberOfCoins = Random.Range(3, 8);
        for (int i = 0; i < numberOfCoins; i++)
        {
            int spawnerNumber = Random.Range(0, spawners.Length);
            Vector2 spawnerPosition = spawners[spawnerNumber].position;

            // Check if the position is already occupied within a tolerance
            while (occupiedPositions.Contains(spawnerPosition))
            {
                spawnerPosition += new Vector2(0, offset);
            }

            // Instantiate the coin at the calculated position
            Instantiate(coinPrefab, spawnerPosition, Quaternion.identity);

            // Mark the position as occupied
            occupiedPositions.Add(spawnerPosition);
        }

        // Clear the occupied positions for the next group of coins
        occupiedPositions.Clear();
    }
}

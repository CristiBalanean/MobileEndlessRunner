using UnityEngine;

public class PoliceSpawning : MonoBehaviour
{
    [SerializeField] private GameObject policePrefab;
    [SerializeField] private Transform player;

    [SerializeField] private Transform[] spawnPoints;

    public void SpawnPoliceCars()
    {
        for (int i = 0; i < spawnPoints.Length; i++) 
        {
            float yVariation = Random.Range(-3.5f, -2f);
            SpawnPoliceCar(spawnPoints[i].position.x, player.position.y + yVariation);
        }
    }

    private void SpawnPoliceCar(float x, float y)
    {
        // Instantiate the police car at the specified position
        GameObject policeCar = Instantiate(policePrefab, new Vector3(x, y, 0f), Quaternion.identity);
    }
}

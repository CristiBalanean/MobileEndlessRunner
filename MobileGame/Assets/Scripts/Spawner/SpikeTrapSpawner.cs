using System.Collections;
using UnityEngine;

public class SpikeTrapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spikeTrapPrefab;

    [SerializeField] private Transform[] spawnPoints;

    private float spawnTrapsCounter = 5f;

    private void Awake()
    {
        PoliceEventManager.StartSpawningSpikeTraps += SpawnTraps;
    }

    private void OnDestroy()
    {
        PoliceEventManager.StartSpawningSpikeTraps -= SpawnTraps;
    }

    public IEnumerator SpawnTraps()
    {
        Debug.Log("Started Spawning Spike Traps!");
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Instantiate(spikeTrapPrefab, spawnPoints[randomSpawnPoint].position + new Vector3(0, 10, 0), Quaternion.identity);
        yield return new WaitForSeconds(spawnTrapsCounter);
        StartCoroutine(SpawnTraps());
    }
}

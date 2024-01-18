using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PoliceSpawning : MonoBehaviour
{
    public static PoliceSpawning instance;

    public UnityEvent DisablePoliceEvent;

    [SerializeField] private GameObject[] policePrefab;
    [SerializeField] private Transform player;

    [SerializeField] private Transform[] spawnPoints;

    public int totalNumberOfCarsToSpawn;
    public int currentNumberOfCarsToSpawn;
    public int currentNumberOfCars;

    private void Awake()
    {
        instance = this;
        currentNumberOfCars = 0;
        currentNumberOfCarsToSpawn = totalNumberOfCarsToSpawn;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void SetupPoliceCars()
    {
        Debug.Log("Setup");

        for (int i = 0; i < 3; i++)
        {
            SpawnPoliceCar();
            currentNumberOfCars++;
            currentNumberOfCarsToSpawn--;
        }
    }

    public void SpawnPoliceCar()
    {
        Debug.Log("Spawned");

        float yVariation = Random.Range(-12f, -10f);

        // Randomly select a spawn point
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        float xPosition = selectedSpawnPoint.position.x;
        float yPosition = player.position.y + yVariation;

        int rand = Random.Range(0,policePrefab.Length);
        // Instantiate the police car at the specified position
        Instantiate(policePrefab[rand], new Vector3(xPosition, yPosition, 0f), Quaternion.identity);
    }

    public IEnumerator StartSpawning()
    {
        while (PoliceEvent.instance.hasStarted)
        {
            yield return new WaitForSeconds(5f);
            SpawnPoliceCar();
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

using UnityEngine;

public class PoliceSpawning : MonoBehaviour
{
    public static PoliceSpawning instance;

    [SerializeField] private GameObject[] policePrefab;
    [SerializeField] private Transform player;

    [SerializeField] private Transform[] spawnPoints;
    private GameObject[] activePoliceCars; // Array to keep track of active police cars
    private int activePoliceCount; // Current number of active police cars

    private void Awake()
    {
        instance = this;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        // Initialize the array of active police cars
        activePoliceCars = new GameObject[3];
        activePoliceCount = 0;
    }

    public void SpawnPoliceCars()
    {
        // Spawn police cars only if there are less than 2 active police cars
        while (activePoliceCount < 3 && PoliceEvent.instance.currentNumberOfCars > 0)
        {
            float yVariation = Random.Range(-12f, -10f);

            // Randomly select a spawn point
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            float xPosition = selectedSpawnPoint.position.x;
            float yPosition = player.position.y + yVariation;

            int rand = Random.Range(0,policePrefab.Length);
            // Instantiate the police car at the specified position
            GameObject policeCar = Instantiate(policePrefab[rand], new Vector3(xPosition, yPosition, 0f), Quaternion.identity);

            // Add the police car to the array of active police cars
            activePoliceCars[activePoliceCount] = policeCar;
            activePoliceCount++;
        }
    }

    public void RemovePoliceCar(GameObject car)
    {
        PoliceEvent.instance.currentNumberOfCars--;

        // Remove the destroyed police car from the array of active police cars
        for (int i = 0; i < activePoliceCount; i++)
        {
            if (activePoliceCars[i] == car)
            {
                activePoliceCars[i] = null;
                activePoliceCount--;
                break;
            }
        }

        if (PoliceEvent.instance.hasStarted)
            SpawnPoliceCars(); // Spawn a new police car to maintain the count of active police cars
        else
        {
            PoliceEvent.instance.StartSpawningCars?.Invoke();
            ScoreManager.Instance.AddToScore(25000);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

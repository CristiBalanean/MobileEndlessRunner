using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PoliceEventManager : MonoBehaviour
{
    public UnityEvent StopSpawningCars;
    public UnityEvent StartSpawningCars;
    public UnityEvent SpawnBarrier;
    public UnityEvent ChangeCameraOffsetToPolice;
    public UnityEvent ChangeCameraOffsetToNormal;

    public delegate void StartSpawningSpikeTrapsDelegate();
    public static event StartSpawningSpikeTrapsDelegate StartSpawningSpikeTraps;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] PoliceSpawning policeSpawning;

    [SerializeField] private float eventDuration;

    private bool hasStarted = false;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void PoliceEventChance()
    {
        float rand = Random.Range(0, 1000);

        if (rand < 30 && !hasStarted && !CarMovement.Instance.hasDied)
        {
            Debug.Log("Event Is Preparing!");
            hasStarted = true;
            StopSpawningCars?.Invoke();
            StartCoroutine(WaitForCarsToDespawn());
        }
    }

    private IEnumerator WaitForCarsToDespawn()
    {
        while (Physics2D.OverlapCircleAll(player.position, 50f, obstacleLayer).Length > 0)
        {
            yield return null;
        }
        
        StartCoroutine(SpawnPoliceEvent());
    }

    private IEnumerator SpawnPoliceEvent()
    {
        Debug.Log("Event Has Started!");
        policeSpawning.SpawnPoliceCars();
        ChangeCameraOffsetToPolice?.Invoke();
        StartSpawningSpikeTraps?.Invoke();
        yield return new WaitForSeconds(eventDuration);
        StartSpawningSpikeTraps?.Invoke();
        ChangeCameraOffsetToNormal?.Invoke();
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach(GameObject trap in traps)
        {
            Destroy(trap);
        }
        yield return new WaitForSeconds(1f);
        SpawnBarrier?.Invoke();
        Debug.Log("Event Has Ended!");
        yield return new WaitForSeconds(3f);
        StartSpawningCars?.Invoke();
        ScoreManager.Instance.AddToScore(5000);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}

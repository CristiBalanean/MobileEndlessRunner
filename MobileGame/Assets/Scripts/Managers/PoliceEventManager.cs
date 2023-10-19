using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PoliceEventManager : MonoBehaviour
{
    public UnityEvent StopSpawningCars;
    public UnityEvent StartSpawningCars;
    public UnityEvent SpawnBarrier;

    public delegate IEnumerator StartSpawningSpikeTrapsDelegate();
    public static event StartSpawningSpikeTrapsDelegate StartSpawningSpikeTraps;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] PoliceSpawning policeSpawning;

    [SerializeField] private float eventDuration;

    private bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void PoliceEventChance()
    {
        float rand = Random.Range(0, 1000);

        if (rand < 30 && !hasStarted)
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
        StartCoroutine(StartSpawningSpikeTraps());
        yield return new WaitForSeconds(eventDuration);
        SpawnBarrier?.Invoke();
        StopCoroutine(StartSpawningSpikeTraps());
        Debug.Log("Event Has Ended!");
    }
}

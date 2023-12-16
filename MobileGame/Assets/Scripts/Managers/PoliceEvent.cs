using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class PoliceEvent : MonoBehaviour
{
    public static PoliceEvent instance;

    public UnityEvent StopSpawningCars;
    public UnityEvent StartSpawningCars;
    public UnityEvent SpawnTraps;
    public UnityEvent ChangeCameraOffsetToPolice;
    public UnityEvent ChangeCameraOffsetToNormal;

    [SerializeField] private int numberOfCars;
    public int currentNumberOfCars;

    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] PoliceSpawning policeSpawning;

    public bool hasStarted;

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
        currentNumberOfCars = numberOfCars;
        hasStarted = false;

        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void PoliceEventChance()
    {
        float rand = Random.Range(0, 1000);

        if (rand < 50 && !CarMovement.Instance.hasDied)
        {
            Debug.Log("Event Is Preparing!");
            StopSpawningCars?.Invoke();
            StartCoroutine(WaitForCarsToDespawn());
        }
    }

    private IEnumerator WaitForCarsToDespawn()
    {
        while (Physics2D.OverlapCircleAll(CarMovement.Instance.transform.position, 50f, obstacleLayer).Length > 0)
        {
            yield return null;
        }

        hasStarted = true;
        StartCoroutine(StartEvent());
    }

    private IEnumerator StartEvent()
    {
        CancelInvoke("PoliceEventChance");
        hasStarted = true;
        currentNumberOfCars = numberOfCars;
        policeSpawning.SpawnPoliceCars();
        while (currentNumberOfCars > 3) 
        {
            yield return new WaitForSeconds(7.5f);
            SpawnTraps?.Invoke();
        }
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject trap in traps)
        {
            Destroy(trap);
        }
        Debug.Log("Event Has Ended!");
        hasStarted = false;
        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        if (enabled)
            InvokeRepeating("PoliceEventChance", 30f, 2.5f);
        else
            CancelInvoke("PoliceEventChance");
    }
}

using System.Collections;
using System.Collections.Generic;
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
    public UnityEvent BackDownEvent;

    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private PoliceSpawning policeSpawning;

    [SerializeField] private GameObject carsUpAheadText;
    [SerializeField] private GameObject helicopter;

    public bool hasStarted;
    public bool canSpawnTraps;
    private bool isPreparing;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hasStarted = false;

        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void PoliceEventChance()
    {
        float rand = Random.Range(0, 1000);

        if (rand < 50 && !CarMovement.Instance.hasDied && !isPreparing && !hasStarted)
        {
            Debug.Log("Event Is Preparing!");
            isPreparing = true;
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
        Debug.Log("Event Started!");
        CancelInvoke("PoliceEventChance");
        isPreparing = false;
        hasStarted = true;
        TutorialManager.instance.ShowPopUpPolice();
        ChangeCameraOffsetToPolice?.Invoke();
        policeSpawning.SetupPoliceCars();
        StartCoroutine(policeSpawning.StartSpawning());
        SpawnHelicopter();
        StartCoroutine(SpawnTrapsCoroutine());
        canSpawnTraps = true;
        yield return new WaitForSeconds(30f);
        canSpawnTraps = false;
        hasStarted = false;
        yield return new WaitForSeconds(5f);
        DestroyProps();
        EndEvent();
    }

    public void EndEvent()
    {
        Debug.Log("Event Has Ended!");
        BackDownEvent?.Invoke();
        carsUpAheadText.SetActive(true);
        var animator = carsUpAheadText.GetComponent<Animator>();
        animator.Rebind();
        animator.Update(0f);
        StartCoroutine(StartSpawningCarsCoroutine());
    }

    private void DestroyProps()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject trap in traps)
        {
            Destroy(trap);
        }
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject barrier in barriers)
        {
            Destroy(barrier);
        }
        GameObject[] policeBarriers = GameObject.FindGameObjectsWithTag("PoliceBarrier");
        foreach (GameObject policeBarrier in policeBarriers)
        {
            Destroy(policeBarrier);
        }
    }

    private IEnumerator StartSpawningCarsCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        StartSpawningCars?.Invoke();
        ChangeCameraOffsetToNormal?.Invoke();
        InvokeRepeating("PoliceEventChance", 30f, 2.5f);
    }

    private void SpawnHelicopter()
    {
        Vector2 targetPositionWithOffset = (Vector2)CarMovement.Instance.transform.position - new Vector2(0, 15f);
        Instantiate(helicopter, targetPositionWithOffset, Quaternion.identity);
    }

    private IEnumerator SpawnTrapsCoroutine()
    {
        while (hasStarted)
        {
            yield return new WaitForSeconds(5f);
            SpawnTraps?.Invoke();
        }
    }
}

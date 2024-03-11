using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatSpawner : MonoBehaviour
{
    public static SwatSpawner instance;

    [SerializeField] private GameObject[] barrierPrefab;
    [SerializeField] private GameObject[] swatVehiclePrefab;
    [SerializeField] private GameObject swatRammerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private RectTransform exclamationMark1;
    [SerializeField] private RectTransform exclamationMark2;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private float repeatRate;

    private float warningTime = 1.5f; // Time before spike trap spawns to show the warning

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnPoliceCars", 10f, 5f);
        StartCoroutine(ChooseAction());
    }

    public void SpawnPoliceCars()
    {
            // Randomly select a spawn point
            Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 spawnPointPosition = selectedSpawnPoint.position - new Vector3(0, 25, 0);

            int rand = Random.Range(0,swatVehiclePrefab.Length);
            // Instantiate the police car at the specified position
            GameObject policeCar = Instantiate(swatVehiclePrefab[rand], spawnPointPosition, Quaternion.identity);
    }

    private void SpawnRammer()
    {
        int rand1 = Random.Range(0, spawnPoints.Length);
        Vector3 desiredPosition1 = spawnPoints[rand1].position;
        SoundManager.instance.Play("Warning");

        // Start the PlaceExclamationMark coroutine and wait for it to finish
        StartCoroutine(PlaceExclamationMark(desiredPosition1, () =>
        {
            Vector3 position1 = spawnPoints[rand1].position + new Vector3(0, 10, 0);
            Instantiate(swatRammerPrefab, position1, Quaternion.identity);
        }));
    }

    private void SpawnBarrier()
    {
        int rand = Random.Range(1, 3);
        Vector3 desiredPosition = spawnPoints[rand].position;
        SoundManager.instance.Play("Warning");

        // Start the PlaceExclamationMark coroutine and wait for it to finish
        StartCoroutine(PlaceExclamationMark(desiredPosition, () =>
        {
            Vector3 position = spawnPoints[rand].position + new Vector3(0, 10, 0);
            int randomPrefab = Random.Range(0, barrierPrefab.Length);
            Instantiate(barrierPrefab[randomPrefab], position, Quaternion.identity);
        }));
    }

    private IEnumerator ChooseAction()
    {
        yield return new WaitForSeconds(repeatRate + 20);
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            SpawnRammer();
        }
        else
        {
            SpawnBarrier();
        }
        StartCoroutine(ChooseAction());
    }

    private IEnumerator PlaceExclamationMark(Vector2 position, System.Action onExclamationMarkComplete)
    {
        Vector3 screenPosition1 = Camera.main.WorldToScreenPoint(position);
        Vector2 canvasSpace1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition1, null, out canvasSpace1);
        exclamationMark1.anchoredPosition = new Vector2(canvasSpace1.x, -250);
        exclamationMark1.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        exclamationMark1.gameObject.SetActive(false);

        // Invoke the callback to signal that the exclamation mark is complete
        onExclamationMarkComplete.Invoke();
    }
}

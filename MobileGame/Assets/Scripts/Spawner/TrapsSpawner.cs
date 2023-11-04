using System.Collections;
using UnityEngine;

public class TrapsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] trapsPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private RectTransform exclamationMark1;
    [SerializeField] private RectTransform exclamationMark2;
    [SerializeField] private RectTransform canvas;

    private float spawnTrapsCounter = 5f;
    private float warningTime = 1f; // Time before spike trap spawns to show the warning
    private bool canSpawn = false;

    private void Awake()
    {
        PoliceEventManager.StartSpawningSpikeTraps += SpawningTrapsState;
    }

    private void OnDestroy()
    {
        PoliceEventManager.StartSpawningSpikeTraps -= SpawningTrapsState;
    }

    public void SpawningTrapsState()
    {
        if (!canSpawn)
        {
            canSpawn = true;
            StartCoroutine(SpawnTraps());
        }
        else
            canSpawn = false;
    }

    private IEnumerator SpawnTraps()
    {
        while (canSpawn)
        {
            Debug.Log("Started Spawning Spike Traps!");
            int randomSpawnPoint1 = Random.Range(0, spawnPoints.Length);
            int randomSpawnPoint2 = Random.Range(0, spawnPoints.Length);

            while (randomSpawnPoint1 == randomSpawnPoint2)
            {
                randomSpawnPoint2 = Random.Range(0, spawnPoints.Length);
            }

            yield return new WaitForSeconds(spawnTrapsCounter - warningTime);
            // Show exclamation mark
            exclamationMark1.gameObject.SetActive(true);
            exclamationMark2.gameObject.SetActive(true);

            Vector3 desiredPosition1 = spawnPoints[randomSpawnPoint1].position;
            Vector3 screenPosition1 = Camera.main.WorldToScreenPoint(desiredPosition1);
            Vector2 canvasSpace1;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition1, null, out canvasSpace1);
            exclamationMark1.anchoredPosition = new Vector2(canvasSpace1.x, -250);

            Vector3 desiredPosition2 = spawnPoints[randomSpawnPoint2].position;
            Vector3 screenPosition2 = Camera.main.WorldToScreenPoint(desiredPosition2);
            Vector2 canvasSpace2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition2, null, out canvasSpace2);
            exclamationMark2.anchoredPosition = new Vector2(canvasSpace2.x, -250);

            yield return new WaitForSeconds(warningTime);
            // Hide exclamation mark
            exclamationMark1.gameObject.SetActive(false);
            exclamationMark2.gameObject.SetActive(false);

            // Spawn spike trap
            int rand1 = Random.Range(0, trapsPrefab.Length);
            int rand2 = Random.Range(0, trapsPrefab.Length);
            Instantiate(trapsPrefab[rand1], spawnPoints[randomSpawnPoint1].position + new Vector3(0, 10, 0), Quaternion.identity);
            Instantiate(trapsPrefab[rand2], spawnPoints[randomSpawnPoint2].position + new Vector3(0, 10, 0), Quaternion.identity);
        }
    }
}

using System.Collections;
using UnityEngine;

public class TrapsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] trapsPrefab;
    [SerializeField] private GameObject barrierPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private RectTransform exclamationMark1;
    [SerializeField] private RectTransform exclamationMark2;
    [SerializeField] private RectTransform canvas;

    private float warningTime = 1f; // Time before spike trap spawns to show the warning

    public void SpawnTraps()
    {
        int randomTrap = Random.Range(0, 2);

        if (randomTrap == 0)
        {
            //we spawn spike traps
            int randomSpawnPoint1 = Random.Range(0, spawnPoints.Length);
            int randomSpawnPoint2 = Random.Range(0, spawnPoints.Length);
            while (randomSpawnPoint1 == randomSpawnPoint2)
            {
                randomSpawnPoint2 = Random.Range(0, spawnPoints.Length);
            }

            StartCoroutine(PlaceExclamationMark(exclamationMark1, spawnPoints[randomSpawnPoint1].position, () =>
            {
                Instantiate(trapsPrefab[0], spawnPoints[randomSpawnPoint1].position + new Vector3(0, 10, 0), Quaternion.identity);
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (i != randomSpawnPoint1 && i != randomSpawnPoint2)
                    {
                        Instantiate(barrierPrefab, spawnPoints[i].position + new Vector3(0, 10, 0), Quaternion.identity);
                    }
                }
            }));
            StartCoroutine(PlaceExclamationMark(exclamationMark2, spawnPoints[randomSpawnPoint2].position, () =>
            {
                Instantiate(trapsPrefab[0], spawnPoints[randomSpawnPoint2].position + new Vector3(0, 10, 0), Quaternion.identity);
            }));
        }
        else
        {
            //we spawn car barrier
            int randomSpawnPoint1 = Random.Range(1, 3);

            StartCoroutine(PlaceExclamationMark(exclamationMark1, spawnPoints[randomSpawnPoint1].position, () =>
            {
                int rand = Random.Range(1, trapsPrefab.Length);
                Instantiate(trapsPrefab[rand], spawnPoints[randomSpawnPoint1].position + new Vector3(0, 10, 0), Quaternion.identity);
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (i != randomSpawnPoint1)
                    {
                        Instantiate(barrierPrefab, spawnPoints[i].position + new Vector3(0, 10, 0), Quaternion.identity);
                    }
                }
            }));
        }
    }

    private IEnumerator PlaceExclamationMark(RectTransform exclamationMark, Vector2 position, System.Action onExclamationMarkComplete)
    {
        Vector3 screenPosition1 = Camera.main.WorldToScreenPoint(position);
        Vector2 canvasSpace1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition1, null, out canvasSpace1);
        exclamationMark.anchoredPosition = new Vector2(canvasSpace1.x, -250);
        exclamationMark.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        exclamationMark.gameObject.SetActive(false);

        // Invoke the callback to signal that the exclamation mark is complete
        onExclamationMarkComplete.Invoke();
    }
}

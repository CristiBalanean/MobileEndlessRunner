using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawners;

    [SerializeField] private GameObject[] barrierPrefabs;

    [SerializeField] private RectTransform exclamationMark1;
    [SerializeField] private RectTransform exclamationMark2;
    [SerializeField] private RectTransform exclamationMark3;
    [SerializeField] private RectTransform canvas;

    private float warningTime = 1f; // Time before spike trap spawns to show the warning

    public void SpawnBarrier()
    {
        int rand = Random.Range(0, barrierPrefabs.Length);

        switch (rand)
        {
            case 0:
                //two barriers
                StartCoroutine(PlaceExclamationMark(exclamationMark1, spawners[1].position, () =>
                {
                    Instantiate(barrierPrefabs[rand], new Vector3(0, player.transform.position.y + 30, 0), Quaternion.identity);
                }));
                StartCoroutine(PlaceExclamationMark(exclamationMark2, spawners[2].position, () => { return; }));
                break;

            case 1:
                //barrier on right
                StartCoroutine(PlaceExclamationMark(exclamationMark1, spawners[0].position, () =>
                {
                    Instantiate(barrierPrefabs[rand], new Vector3(0, player.transform.position.y + 30, 0), Quaternion.identity);
                }));
                StartCoroutine(PlaceExclamationMark(exclamationMark2, spawners[1].position, () => { return; }));
                StartCoroutine(PlaceExclamationMark(exclamationMark3, spawners[2].position, () => { return; }));
                break;

            case 2:
                //barrier on middle
                StartCoroutine(PlaceExclamationMark(exclamationMark1, spawners[0].position, () =>
                {
                    Instantiate(barrierPrefabs[rand], new Vector3(0, player.transform.position.y + 30, 0), Quaternion.identity);
                }));
                StartCoroutine(PlaceExclamationMark(exclamationMark2, spawners[3].position, () => { return; }));
                break;

            case 3:
                //barrier on left
                StartCoroutine(PlaceExclamationMark(exclamationMark1, spawners[1].position, () =>
                {
                    Instantiate(barrierPrefabs[rand], new Vector3(0, player.transform.position.y + 30, 0), Quaternion.identity);
                }));
                StartCoroutine(PlaceExclamationMark(exclamationMark2, spawners[2].position, () => { return; }));
                StartCoroutine(PlaceExclamationMark(exclamationMark3, spawners[3].position, () => { return; }));
                break;
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

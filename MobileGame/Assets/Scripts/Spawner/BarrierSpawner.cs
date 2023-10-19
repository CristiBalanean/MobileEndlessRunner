using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private GameObject barrierPrefab;

    public void SpawnBarrier()
    {
        Instantiate(barrierPrefab, new Vector3(0, player.transform.position.y + 30, 0), Quaternion.identity);
    }
}

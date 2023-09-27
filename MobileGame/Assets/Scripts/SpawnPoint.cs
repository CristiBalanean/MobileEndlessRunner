using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(obstacle, transform.position, Quaternion.identity);
    }
}

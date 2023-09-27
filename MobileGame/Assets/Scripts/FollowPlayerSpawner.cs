using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerSpawner : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, target.position.y + offset.y);
    }
}

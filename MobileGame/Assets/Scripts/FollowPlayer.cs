using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake()
    {
        target = CarMovement.Instance.transform;
    }

    private void Update()
    {
        transform.position = target.transform.position;
    }
}

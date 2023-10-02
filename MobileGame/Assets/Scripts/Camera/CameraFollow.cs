using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float minFOV;
    [SerializeField] private float maxFOV;
    [SerializeField] private float lowSpeedYOffset;
    [SerializeField] private float highSpeedYOffset;

    [SerializeField] private float desiredYPosition;

    [SerializeField] private Camera mainCamera;

    private CarMovement player;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        player = target.GetComponent<CarMovement>();
    }

    private void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(minFOV, maxFOV, player.GetCameraNormalizedSpeed());
    }

    private void FixedUpdate()
    {
        desiredYPosition = Mathf.Lerp(lowSpeedYOffset, highSpeedYOffset, player.GetCameraNormalizedSpeed());
        Vector3 desiredPosition = target.position + new Vector3(0, desiredYPosition, 0);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, player.GetCameraNormalizedSpeed());
        transform.position = new Vector3(transform.position.x, smoothedPosition.y, transform.position.z);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;

    private float minSpeed;
    private float maxSpeed;
    private float currentSpeed;

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float pitchFromCar;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        minSpeed = 25;
        maxSpeed = CarMovement.Instance.GetTopSpeed();
    }

    private void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        currentSpeed = rigidBody.velocity.magnitude * 3.6f;
        pitchFromCar = rigidBody.velocity.magnitude / 30f;

        if (currentSpeed < minSpeed)
            audioSource.pitch = minPitch;

        if(currentSpeed > minSpeed && currentSpeed < maxSpeed)
            audioSource.pitch = minPitch + pitchFromCar;
    }
}

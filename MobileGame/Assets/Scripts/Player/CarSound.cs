using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private AudioSource[] audioSource;
    public AudioSource correctAudioSource;

    private float minSpeed;
    private float maxSpeed;
    private float currentSpeed;

    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float pitchFromCar;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponents<AudioSource>();
        foreach (AudioSource audio in audioSource)
            if (audio.clip == audioClip)
                correctAudioSource = audio;
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
        currentSpeed = CarMovement.Instance.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3.6f;
        pitchFromCar = CarMovement.Instance.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude / 30f;

        if (currentSpeed < minSpeed)
            correctAudioSource.pitch = minPitch;

        if(currentSpeed > minSpeed && currentSpeed < maxSpeed)
            correctAudioSource.pitch = minPitch + pitchFromCar;
    }
}

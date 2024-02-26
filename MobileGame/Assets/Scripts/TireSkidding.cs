using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSkidding : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] skidMarkTrails;
    [SerializeField] private ParticleSystem[] skidMarkParticles;

    private Rigidbody2D rigidBody;
    private float previousVelocityMagnitude;
    private bool isSkidding;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        foreach (ParticleSystem particle in skidMarkParticles)
            particle.Stop();

        previousVelocityMagnitude = rigidBody.velocity.magnitude;

        // Start the coroutine to check velocity change every 0.5 seconds
        StartCoroutine(CheckVelocityChange());
    }

    private IEnumerator CheckVelocityChange()
    {
        while (true)
        {
            CheckForSkid();
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void CheckForSkid()
    {
        float currentVelocityMagnitude = rigidBody.velocity.magnitude;

        // Calculate the change in velocity
        float velocityChange = previousVelocityMagnitude - currentVelocityMagnitude;

        // Check if the change in velocity is significant
        if (velocityChange > 0.35f)  // Adjust this threshold as needed
        {
            float currentSpeed = CarMovement.Instance.GetSpeed();
            if (currentSpeed > 50f)
                StartSkidEffects();
        }
        else
        {
            StopSkidEffects();
        }

        // Update the previous velocity for the next frame
        previousVelocityMagnitude = currentVelocityMagnitude;
    }


    private void StartSkidEffects()
    {
        foreach (TrailRenderer trail in skidMarkTrails)
            trail.emitting = true;

        foreach (ParticleSystem particle in skidMarkParticles)
        {
            particle.Emit(1);
            particle.Play();
        }

        if (!isSkidding)
        {
            if (transform.CompareTag("Police"))
            {
                GetComponent<AudioSource>().Play();
                isSkidding = true;
            }

            if(transform.CompareTag("Player"))
            {
                SoundManager.instance.Play("Skid");
                isSkidding = true;
            }
        }
    }

    private void StopSkidEffects()
    {
        foreach (TrailRenderer trail in skidMarkTrails)
            trail.emitting = false;

        foreach (ParticleSystem particle in skidMarkParticles)
            particle.Stop();

        if (isSkidding)
        {
            if (transform.CompareTag("Police"))
            {
                GetComponent<AudioSource>().Stop();
                isSkidding = false;
            }

            if (transform.CompareTag("Player"))
            {
                SoundManager.instance.Stop("Skid");
                isSkidding = false;
            }
        }
    }
}

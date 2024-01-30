using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRainParticle : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void StartParticle()
    {
        particle.Play();
    }

    public void StopParticle()
    {
        particle.Stop();
    }
}

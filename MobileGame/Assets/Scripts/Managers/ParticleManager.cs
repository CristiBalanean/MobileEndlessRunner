using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [SerializeField] private Particle[] particles;

    private void Awake()
    {
        instance = this;
    }

    public void InstantiateParticle(Transform position, string name)
    {
        foreach(var particle in particles)
        {
            if(particle.name == name)
            {
                GameObject particleEffect = Instantiate(particle.particleGO, position.position, Quaternion.identity);
                particleEffect.transform.parent = GameObject.Find("Player").transform;
            }
        }
    }
}

public class Particle
{
    public GameObject particleGO;
    public string name;
}


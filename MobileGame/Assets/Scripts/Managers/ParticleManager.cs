using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [SerializeField] private GameObject smokeParticle;

    private void Awake()
    {
        instance = this;
    }

    public void InstantiateParticle(Transform position)
    {
        Instantiate(smokeParticle, position.position + new Vector3(0, 0.36f, 0), Quaternion.identity);
    }
}

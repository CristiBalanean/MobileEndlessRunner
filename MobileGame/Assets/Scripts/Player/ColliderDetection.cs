using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderDetection : MonoBehaviour
{
    private CarHealth health;
    public CameraCollisionShake cameraShake;

    [SerializeField] private GameObject impactParticle;

    private void Awake()
    {
        Transform rootObject = transform.root;
        health = rootObject.GetComponent<CarHealth>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraCollisionShake>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(cameraShake.Shake(.1f, .1f, 1f));
        SoundManager.instance.Play("Crash");

        float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
        health.TakeDamage((int)collisionMagnitude);

        GameObject particle = Instantiate(impactParticle, collision.transform.position, UnityEngine.Quaternion.identity);
        Destroy(particle, 1.1f);
    }
}

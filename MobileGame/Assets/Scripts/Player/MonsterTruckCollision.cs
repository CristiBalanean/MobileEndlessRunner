using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTruckCollision : MonoBehaviour
{
    private CarHealth health;
    public CameraCollisionShake cameraShake;

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

        if(collision.transform.CompareTag("Police"))
        {
            float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
            health.TakeDamage((int)collisionMagnitude);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontColliderDetection : MonoBehaviour
{
    private CarHealth playerHealth;
    public CameraCollisionShake cameraShake;

    private void Start()
    {
        playerHealth = transform.root.GetComponent<CarHealth>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraCollisionShake>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(cameraShake.Shake(.1f, .1f, 1f));
        SoundManager.instance.Play("Crash");

        AIHealth aiHealth = collision.gameObject.GetComponent<AIHealth>();
        if (aiHealth != null)
        {
            StartCoroutine(aiHealth.DeathTrigger());
            if (SceneManager.GetActiveScene().name == "MonsterTruckGameMode")
                aiHealth.TriggerExplosion();
        }

        Damageable currentCollision = collision.gameObject.GetComponent<Damageable>();
        float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
        if (currentCollision != null && playerHealth != null)
        {
            if (collisionMagnitude > 20)
            {
                playerHealth.TriggerDeath();
                return;
            }
            else
                playerHealth.TakeDamage((int)collisionMagnitude);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetectionAI : MonoBehaviour
{
    private PoliceAiHealth policeHealth;
    private AIHealth health;

    private void Awake()
    {
        policeHealth = GetComponent<PoliceAiHealth>();
        health = GetComponent<AIHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(policeHealth != null)
        {
            float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
            policeHealth.TakeDamage((int)collisionMagnitude);
        }
        if(health != null)
        {
            StartCoroutine(health.DeathTrigger());
            if (SceneManager.GetActiveScene().name == "MonsterTruckGameMode")
                health.TriggerExplosion();
        }
    }
}

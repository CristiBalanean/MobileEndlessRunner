using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwatAICollision : MonoBehaviour
{
    private PoliceAiHealth policeHealth;

    private void Awake()
    {
        Transform root = transform.root;
        policeHealth = root.GetComponent<PoliceAiHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.instance.Play("Crash");

        if (policeHealth != null)
        {
            if (collision.transform.CompareTag("Player"))
            {
                float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
                policeHealth.TakeDamage((int)collisionMagnitude);
            }
        }
    }
}

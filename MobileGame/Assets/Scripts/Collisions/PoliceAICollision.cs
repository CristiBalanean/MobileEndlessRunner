using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceAICollision : MonoBehaviour
{
    private PoliceAiHealth policeHealth;

    private void Awake()
    {
        Transform root = transform.root;
        policeHealth = GetComponent<PoliceAiHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (policeHealth != null)
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (collision.collider is BoxCollider2D)
                {
                    float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
                    policeHealth.TakeDamage((int)collisionMagnitude);
                }
            }

            if (collision.transform.CompareTag("PoliceBarrier"))
            {
                policeHealth.TakeDamage(100);
                if (Vector2.Distance(transform.position, CarMovement.Instance.transform.position) < 6.5f)
                    SoundManager.instance.Play("Crash");
            }

            /*if (collision.transform.CompareTag("Police"))
            {
                if (collision.collider is BoxCollider2D)
                {
                    float collisionMagnitude = Mathf.Abs(collision.relativeVelocity.y);
                    policeHealth.TakeDamage((int)collisionMagnitude);
                    if (Vector2.Distance(transform.position, CarMovement.Instance.transform.position) < 6.5f)
                        SoundManager.instance.Play("Crash");
                }
            }*/
        }
    }
}

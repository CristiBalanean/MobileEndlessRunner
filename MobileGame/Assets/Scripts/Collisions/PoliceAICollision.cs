using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceAICollision : MonoBehaviour
{
    private PoliceAiHealth policeHealth;
    private CameraCollisionShake cameraShake;
    private PoliceAIStateManager policeAIStateManager;

    private void Awake()
    {
        Transform root = transform.root;
        policeHealth = GetComponent<PoliceAiHealth>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraCollisionShake>();
        policeAIStateManager = GetComponent<PoliceAIStateManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (policeHealth != null)
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (SceneManager.GetActiveScene().name == "MonsterTruckGameMode")
                {
                    float collisionIntensity = collision.relativeVelocity.magnitude;
                    policeHealth.TakeDamage((int)collisionIntensity);
                }
                else
                {
                    if (collision.collider is BoxCollider2D)
                    {
                        int chance = Random.Range(0, 100);
                        if (chance < 10)
                        {
                            policeAIStateManager.SwitchState(policeAIStateManager.crashState);
                        }
                    }
                }
            }

            if (collision.transform.CompareTag("PoliceBarrier"))
            {
                policeHealth.TakeDamage(100);
                if (Vector2.Distance(transform.position, CarMovement.Instance.transform.position) < 6.5f)
                    SoundManager.instance.Play("Crash");
            }

            if (collision.transform.CompareTag("Police"))
            {
                float distance = Vector2.Distance(transform.position, CarMovement.Instance.transform.position);
                if (distance < 5f)
                {
                    //StartCoroutine(cameraShake.Shake(.1f, .1f, 1f));
                    SoundManager.instance.Play("Crash");
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CarAICollision : MonoBehaviour
{
    private AIHealth health;

    private void Awake()
    {
        Transform root = transform.root;
        health = root.GetComponent<AIHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.instance.Play("Crash");

        if (health != null)
        {
            StartCoroutine(health.DeathTrigger());
            if (SceneManager.GetActiveScene().name == "MonsterTruckGameMode" && collision.transform.CompareTag("Player"))
                health.TriggerExplosion();
        }
    }
}

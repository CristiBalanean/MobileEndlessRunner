using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceAiHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Material beginningMaterial;
    [SerializeField] private Material explodedCarMaterial;
    [SerializeField] private GameObject explosionGO;
    [SerializeField] private LayerMask obstacleLayer;

    public CameraCollisionShake cameraShake;
    private Animator aiAnimator;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] aiCollider;
    private bool isDead = false; // Flag to prevent multiple calls to DeathTrigger

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAnimator = GetComponent<Animator>();
        aiCollider = GetComponentsInChildren<Collider2D>();
        aiAnimator.enabled = false;
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraCollisionShake>();
    }

    private void OnEnable()
    {
        spriteRenderer.material = beginningMaterial;
        aiAnimator.enabled = false;
        aiAnimator.Rebind();
        aiAnimator.Update(0f);
        foreach (var collider in aiCollider)
            collider.isTrigger = false;
        isDead = false; // Reset the isDead flag when the car is re-enabled
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0 && !isDead)
        {
            isDead = true; // Set the flag to prevent multiple calls
            StartCoroutine(DeathTrigger());
        }
    }

    public IEnumerator DeathTrigger()
    {
        Collider2D[] nearbyCars = Physics2D.OverlapCircleAll(transform.position, 3.5f, obstacleLayer);

        // Handle the death and explosion for each nearby car
        foreach (Collider2D car in nearbyCars)
        {
            PoliceAiHealth carHealth = car.transform.root.GetComponent<PoliceAiHealth>();

            if (carHealth != null && !carHealth.isDead) // Check if the car is not already dead
            {
                carHealth.TakeDamage(100); // Apply the same damage to nearby cars
            }
        }

        TriggerExplosion();

        yield return new WaitForSeconds(0.01f);
        aiAnimator.enabled = true;
        foreach (var collider in aiCollider)
            collider.isTrigger = true;
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);

        if (SwatSpawner.instance != null)
            SwatSpawner.instance.RemovePoliceCar(gameObject);
        else if (PoliceSpawning.instance != null)
            PoliceSpawning.instance.RemovePoliceCar(gameObject);
    }

    public void TriggerExplosion()
    {
        StartCoroutine(cameraShake.Shake(.5f, .5f, 1f));
        ScoreManager.Instance.AddToScore(2500);
        spriteRenderer.material = explodedCarMaterial;
        GameObject explosion = Instantiate(explosionGO, transform.position, Quaternion.identity);
        SoundManager.instance.Play("Explosion");
        Destroy(explosion, 1.25f);
    }
}

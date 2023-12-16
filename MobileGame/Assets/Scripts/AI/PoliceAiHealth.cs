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

    private Animator aiAnimator;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] aiCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiAnimator = GetComponent<Animator>();
        aiCollider = GetComponentsInChildren<Collider2D>();
        aiAnimator.enabled = false;
    }

    private void OnEnable()
    {
        spriteRenderer.material = beginningMaterial;
        aiAnimator.enabled = false;
        aiAnimator.Rebind();
        aiAnimator.Update(0f);
        foreach (var collider in aiCollider)
            collider.isTrigger = false;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            StartCoroutine(DeathTrigger());
            if (SceneManager.GetActiveScene().name == "MonsterTruckGameMode")
                TriggerExplosion();
        }
    }

    public IEnumerator DeathTrigger()
    {
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
        ScoreManager.Instance.AddToScore(2500);
        spriteRenderer.material = explodedCarMaterial;
        GameObject explosion = Instantiate(explosionGO, transform.position, Quaternion.identity);
        SoundManager.instance.Play("Explosion");
        Destroy(explosion, 1.25f);
    }
}

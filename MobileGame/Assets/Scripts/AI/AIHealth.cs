using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private Material beginningMaterial;
    [SerializeField] private Material explodedCarMaterial;
    [SerializeField] private GameObject explosionGO;

    private Animator aiAnimator;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] aiCollider;

    private void Awake()
    {
        aiAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiCollider = GameObject.FindGameObjectWithTag("AiCollider").GetComponents<Collider2D>();
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

    public IEnumerator DeathTrigger()
    {
        yield return new WaitForSeconds(0.01f);
        aiAnimator.enabled = true;
        foreach (var collider in aiCollider)
            collider.isTrigger = true;
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false);
    }

    public void TriggerExplosion()
    {
        ScoreManager.Instance.AddToScore(5000);
        spriteRenderer.material = explodedCarMaterial;
        GameObject explosion = Instantiate(explosionGO, transform.position, Quaternion.identity);
        SoundManager.instance.Play("Explosion");
        Destroy(explosion, 1.25f);
    }
}

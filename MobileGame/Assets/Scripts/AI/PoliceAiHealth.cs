using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAiHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Material explodedCarMaterial;
    [SerializeField] private GameObject explosionGO;

    private SpriteRenderer spriteRenderer;
    public bool hasDied = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            hasDied = true;
            spriteRenderer.material = explodedCarMaterial;
            Instantiate(explosionGO, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f);
        }
    }
}

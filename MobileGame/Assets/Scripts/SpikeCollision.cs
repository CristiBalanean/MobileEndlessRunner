using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform root = collision.transform.root;
        Rigidbody2D rb = root.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(Vector2.down * 100, ForceMode2D.Force);
    }
}

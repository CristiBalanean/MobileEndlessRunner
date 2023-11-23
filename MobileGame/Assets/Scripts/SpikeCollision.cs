using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.down * 1000, ForceMode2D.Force);
    }
}

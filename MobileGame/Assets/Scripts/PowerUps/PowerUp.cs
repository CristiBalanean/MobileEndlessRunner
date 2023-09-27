using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] protected string powerUpName;
    [SerializeField] protected string powerUpDescription;

    [SerializeField] protected float duration;

    protected void FixedUpdate()
    {
        transform.Translate(Vector2.up * 15f * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            StartCoroutine(PickUp(collision));
    }

    protected virtual IEnumerator PickUp(Collider2D player)
    {
        yield return 0;
    }
}

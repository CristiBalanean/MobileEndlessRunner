using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskyOvertake : MonoBehaviour
{
    private int overlappingColliders = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AiCollider"))
        {
            overlappingColliders++;
            // You can optionally check for specific conditions before allowing risky overtake
            // For example: if (transform.position.y < collision.transform.position.y) { ... }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AiCollider"))
        {
            overlappingColliders--;
            if (overlappingColliders < 0)
            {
                overlappingColliders = 0; // Ensure the counter doesn't go negative
            }

            // Check if the player can perform a risky overtake when leaving the collider
            if (overlappingColliders == 0 && transform.position.y > collision.transform.position.y && collision.isTrigger == false)
            {
                ScoreManager.Instance.IncrementOvertakeCounter();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RiskyOvertake : MonoBehaviour
{
    private bool hasCollided = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasCollided && collision.CompareTag("Obstacle") && transform.position.y > collision.transform.position.y)
        {
            ScoreManager.Instance.IncrementOvertakeCounter();
            hasCollided = true; // Set the flag to true to prevent further calls during this collision
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            hasCollided = false; // Reset the flag when the objects are no longer colliding
        }
    }
}

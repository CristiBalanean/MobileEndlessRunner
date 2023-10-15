using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RiskyOvertake : MonoBehaviour
{
    public bool canRiskyOvertake = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.y < collision.transform.position.y && collision.CompareTag("Obstacle"))
            canRiskyOvertake = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canRiskyOvertake && transform.position.y > collision.transform.position.y)
            ScoreManager.Instance.IncrementOvertakeCounter();

        canRiskyOvertake = false;
    }
}

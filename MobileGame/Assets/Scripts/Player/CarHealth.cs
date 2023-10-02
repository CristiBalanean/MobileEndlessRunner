using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent deathTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Police"))
        {
            deathTrigger.Invoke();

            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimePickup : MonoBehaviour
{
    [SerializeField] private float pickupSpeed;

    public UnityEvent pickupEvent;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        TimeMode timeMode = FindObjectOfType<TimeMode>();
        pickupEvent.AddListener(timeMode.ReplenishTime);
    }

    private void Start()
    {
        rigidBody.velocity = Vector2.up * pickupSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform parent = collision.transform.root;
        if (parent.GetComponent<TimeMode>() != null)
        {
            SoundManager.instance.Play("Pickup");
            pickupEvent.Invoke();
            Destroy(gameObject);
        }
    }
}

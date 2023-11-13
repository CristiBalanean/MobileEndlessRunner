using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GasPickup : MonoBehaviour
{
    [SerializeField] private float pickupSpeed;

    public UnityEvent pickupEvent;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        MonsterTruckGas monsterTruckGas = FindObjectOfType<MonsterTruckGas>();
        pickupEvent.AddListener(monsterTruckGas.ReplenishGas);
    }

    private void Start()
    {
        rigidBody.velocity = Vector2.up * pickupSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<MonsterTruckGas>() != null)
        {
            pickupEvent.Invoke();
            Destroy(gameObject);
        }
    }
}

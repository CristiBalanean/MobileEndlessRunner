using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatRammer : MonoBehaviour
{
    [SerializeField] private float acceleration;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody.velocity = Vector2.down * acceleration;
    }
}

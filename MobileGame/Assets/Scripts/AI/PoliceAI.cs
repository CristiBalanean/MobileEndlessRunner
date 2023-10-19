using System.Collections;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    private CarMovement player;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CarMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody.velocity = (player.GetSpeed() / 3.6f) * Vector2.up;
    }

    private void FixedUpdate()
    {
        if (rigidBody.velocity.y < player.GetSpeed() / 3.6f)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, player.GetSpeed() / 3.6f);
        }
    }
}

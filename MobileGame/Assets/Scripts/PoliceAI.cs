using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private CarMovement playerMovement;
    private bool canChase;

    [SerializeField] float startingSpeed;
    [SerializeField] float minDistance;
    [SerializeField] float maxAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float brakeForce;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<CarMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody.velocity = Vector2.up * startingSpeed;
        canChase = false;

        InvokeRepeating("StartCheck", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (canChase)
        {
            GetComponent<AIBraking>().enabled = false;

            float playerY = playerMovement.transform.position.y;
            float policeY = transform.position.y;

            if (playerY - 1.5f < policeY)
            {
                return;
            }
            else
            {
                float targetY = Mathf.Lerp(transform.position.y, playerMovement.transform.position.y, Time.fixedDeltaTime * acceleration * 0.5f);
                float targetX = Mathf.Lerp(transform.position.x, playerMovement.transform.position.x, Time.fixedDeltaTime * acceleration * 0.5f);
                transform.position = new Vector2(targetX, targetY);

                if (acceleration < maxAcceleration)
                    acceleration += Time.fixedDeltaTime;
            }
        }
    }

    private void StartCheck()
    {
        CheckDistance(playerMovement.transform);
    }

    private void CheckDistance(Transform player)
    {
        if (Vector2.Distance(transform.position, player.position) < minDistance)
        {
            canChase = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            if (canChase)
                ScoreManager.Instance.AddToScore(100);

            Destroy(gameObject);
        }
    }
}

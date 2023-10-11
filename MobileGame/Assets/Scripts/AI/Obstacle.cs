using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float topSpeed;
    [SerializeField] private float acceleration;

    public float currentSpeed;

    private Rigidbody2D rigidBody;

    private GameObject player;

    private bool shouldCheckDistance = false;
    [SerializeField] private bool aggresiveDriver = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = SpritePool.Instance.ChooseSprite();

        // Set the initial velocity
        Vector2 initialVelocity = Vector2.up * topSpeed;
        rigidBody.velocity = initialVelocity;
        currentSpeed = topSpeed;

        // Start checking distance when enabled
        shouldCheckDistance = true;

        int rand = Random.Range(0, 2);
        if (rand == 0)
            aggresiveDriver = false;
        else
            aggresiveDriver = true;
    }

    private void Update()
    {
        // Check distance only when shouldCheckDistance is true
        if (shouldCheckDistance)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (player.transform.position.y < transform.position.y && distance > 25f)
                gameObject.SetActive(false);
            else if (player.transform.position.y > transform.position.y && distance > 10f)
                gameObject.SetActive(false);
        }
    }
}

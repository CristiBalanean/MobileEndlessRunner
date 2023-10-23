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

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        CarGraphics carGraphics = SpritePool.Instance.ChooseSprite();
        GetComponent<SpriteRenderer>().sprite = carGraphics.GetSprite();
        GameObject collider = Instantiate(carGraphics.GetCollider(), transform.position, Quaternion.identity);
        collider.transform.parent = transform;
    }

    private void OnEnable()
    {
        // Set the initial velocity
        Vector2 initialVelocity = Vector2.up * topSpeed;
        rigidBody.velocity = initialVelocity;
        currentSpeed = topSpeed;

        // Start checking distance when enabled
        shouldCheckDistance = true;
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

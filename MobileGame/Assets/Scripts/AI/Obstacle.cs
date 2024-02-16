using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float topSpeed;
    [SerializeField] private float acceleration;

    public float currentSpeed;

    private Rigidbody2D rigidBody;

    private GameObject player;

    private bool shouldCheckDistance = false;

    public static bool firstCar = false;

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
        Vector2 localForward = transform.up; // Assuming the car's forward direction is its local up direction

        if (Vector2.Dot(localForward, Vector2.down) < 0)
        {
            float speedRatio = CarMovement.Instance.GetTopSpeed() / 310;
            topSpeed = Random.Range(CarMovement.Instance.maxYVelocity - 5, CarMovement.Instance.maxYVelocity) - Mathf.Lerp(10, 15, speedRatio);
        }
        else
        {
            float speedRatio = CarMovement.Instance.GetTopSpeed() / 310;
            topSpeed = Random.Range(CarMovement.Instance.maxYVelocity - 5, CarMovement.Instance.maxYVelocity) - Mathf.Lerp(20, 25, speedRatio);
        }

        // Set the initial velocity in the determined local forward direction
        Vector2 initialVelocity = localForward * topSpeed;

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

            else if (Vector2.Distance(player.transform.position, transform.position) < 3f && !firstCar)
            {
                TutorialManager.instance.ShowPopUpCars();
                firstCar = true;
            }
        }
    }
}

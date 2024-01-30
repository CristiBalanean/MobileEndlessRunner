using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIStateManager : MonoBehaviour
{
    PoliceAIBaseState currentState;
    public PoliceAIFollowState followState = new PoliceAIFollowState();
    public PoliceAIBlockState blockState = new PoliceAIBlockState();
    public PoliceAIHookState hookState = new PoliceAIHookState();
    public PoliceAIBustState bustState = new PoliceAIBustState();
    public PoliceAICrashingState crashState = new PoliceAICrashingState();

    public Transform target;
    public Rigidbody2D rigidBody;
    public Rigidbody2D targetRigidbody;
    public PoliceAIStateManager state;
    public AvoidanceBehavior avoidance;
    public CameraCollisionShake cameraShake;
    public Collider2D aiCollider;
    public Material hookMaterial;

    public float kd;
    public float kp;

    private bool isHooked;

    private void Awake()
    {
        target = CarMovement.Instance.transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        state = GetComponent<PoliceAIStateManager>();
        avoidance = GetComponent<AvoidanceBehavior>();
        aiCollider = GetComponentInChildren<Collider2D>();
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraCollisionShake>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        currentState = followState;

        currentState.EnterState(this);

        InvokeRepeating("CheckForDespawn", 5f, 1f);
    }

    private void Update()
    {
        if(currentState != followState)
            currentState.UpdateState(this);
    }

    private void FixedUpdate() 
    {
        if(currentState == followState)
            currentState.UpdateState(this);
    }

    public void SwitchState(PoliceAIBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void CheckForDespawn()
    {
        float distance = Vector2.Distance(transform.position, CarMovement.Instance.transform.position);

        if (distance > 15)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == followState && collision.transform.CompareTag("Player"))
        {
            foreach(ContactPoint2D c in collision.contacts)
            {
                if(c.collider.name == "BackCollider" && !isHooked)
                {
                    int chance = Random.Range(0, 100);
                    if (chance < 7)
                    {
                        isHooked = true;
                        this.SwitchState(hookState);
                    }
                }
            }
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
        rigidBody.simulated = newGameState == GameState.Gameplay;
    }
}

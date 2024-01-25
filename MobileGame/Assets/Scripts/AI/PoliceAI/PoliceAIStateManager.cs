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

    public Transform target;
    public Rigidbody2D rigidBody;
    public Rigidbody2D targetRigidbody;

    public float kd;
    public float kp;

    private void Awake()
    {
        target = CarMovement.Instance.transform;
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = followState;

        currentState.EnterState(this);
    }

    private void Update()
    {
        ClampCarPositionHorizontal();

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

    private void ClampCarPositionHorizontal()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
        transform.position = pos;
    }
}

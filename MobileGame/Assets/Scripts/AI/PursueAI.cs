using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueAI : MonoBehaviour
{
    private Transform target;

    private SeekTarget seekTarget;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        seekTarget = GetComponent<SeekTarget>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        target = seekTarget.Seek();
    }

    private void FixedUpdate()
    {
        Pursue();
    }

    private void Pursue()
    {
        if (target == null)
            return;

        Vector2 targetDirection = target.position - this.transform.position;

        float lookAhead = targetDirection.magnitude / (rigidBody.velocity.y + target.root.GetComponent<Rigidbody2D>().velocity.y);

        Vector2 finalPosition = target.transform.position + target.transform.forward * lookAhead;
        rigidBody.AddForce(finalPosition);
    }
}

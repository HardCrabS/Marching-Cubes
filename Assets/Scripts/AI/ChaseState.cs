using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public float chaseRadius = 10f;
    [Tooltip("Distance at which enemy stops chasing player")]
    public float lostRadius = 15f;

    bool isChasing = false;
    AIMovement movement;
    Transform playerTransform;

    private void Start()
    {
        movement = GetComponent<AIMovement>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    public override void OnEnterState()
    {
        Debug.Log("ChaseState OnEnterState");
        movement.SetDestination(playerTransform.position);
        isChasing = true;
    }

    public override void OnExitState()
    {
        Debug.Log("ChaseState OnExitState");
        movement.StopMoving();
        isChasing = false;
    }

    public override StateType DecideTransition()
    {
        float distanceToPlayer = (playerTransform.position - transform.position).magnitude;

        if (distanceToPlayer < chaseRadius)
            return StateType.Chase;
        if (distanceToPlayer < lostRadius && isChasing)
            return StateType.Chase;
        return StateType.Idle;
    }

    public override void Execute()
    {
        movement.SetDestination(playerTransform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lostRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public float attackRadius = 3f;

    GunController gunController;
    AIMovement movement;
    Transform playerTransform;

    private void Start()
    {
        gunController = GetComponent<GunController>();
        movement = GetComponent<AIMovement>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    public override void OnEnterState()
    {
        Debug.Log("AttackState OnEnterState");
    }

    public override void OnExitState()
    {
        Debug.Log("AttackState OnExitState");
    }

    public override StateType DecideTransition()
    {
        float distanceToPlayer = (playerTransform.position - transform.position).magnitude;

        if (distanceToPlayer < attackRadius)
            return StateType.Attack;

        return StateType.Idle;
    }

    public override void Execute()
    {
        movement.LookAt(playerTransform.position);
        gunController.OnTriggerHold();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

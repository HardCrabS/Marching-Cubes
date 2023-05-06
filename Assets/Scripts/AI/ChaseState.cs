using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public float chaseRadius = 10f;
    [Tooltip("Distance at which enemy stops chasing player")]
    public float lostRadius = 15f;

    bool isChasing = false;
    bool hasBeenNotifiedToAttack = false;
    bool hasBeenAttackedByPlayer = false;

    AIMovement movement;
    Transform playerTransform;

    private void Start()
    {
        movement = GetComponent<AIMovement>();
        playerTransform = FindObjectOfType<Player>().transform;

        EventsDispatcher.Instance.onNotifyEnemies += HandleEnemyNotification;
    }

    private void OnDestroy()
    {
        EventsDispatcher.Instance.onNotifyEnemies -= HandleEnemyNotification;
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

        bool chasePlayer = hasBeenAttackedByPlayer ||
                           (distanceToPlayer < chaseRadius) ||
                           (distanceToPlayer < lostRadius && isChasing) ||
                           (distanceToPlayer < lostRadius && hasBeenNotifiedToAttack);

        if (chasePlayer)
            return StateType.Chase;
        return StateType.Idle;
    }

    public override void Execute()
    {
        movement.SetDestination(playerTransform.position);
    }

    void HandleEnemyNotification(EnemyNotification enemyNotification)
    {
        float distanceToNotifyier = (transform.position - enemyNotification.callerPosition).magnitude;
        if (enemyNotification.reason == NotificationReason.NearbyAttack && distanceToNotifyier < lostRadius)
        {
            hasBeenNotifiedToAttack = true;
        }
        else if (enemyNotification.reason == NotificationReason.TookDamage && enemyNotification.caller == GetComponent<AttackState>())
        {
            hasBeenAttackedByPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lostRadius);
    }
}

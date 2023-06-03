using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyNotification
{
    public AttackState caller;
    public Vector3 callerPosition;
    public NotificationReason reason;

    public EnemyNotification(AttackState caller, Vector3 callerPos, NotificationReason reason)
    {
        this.caller = caller;
        this.callerPosition = callerPos;
        this.reason = reason;
    }
}


public enum NotificationReason
{
    NearbyAttack,  // nearby enemy started attacking player
    TookDamage  // got hit by player
}


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
        playerTransform = Player.Instance.transform;
    }

    public override void OnEnterState()
    {
        EnemyNotification enemyNotification = new EnemyNotification(this, transform.position, NotificationReason.NearbyAttack);
        EventsDispatcher.Instance.onNotifyEnemies?.Invoke(enemyNotification);
    }

    public override void OnExitState()
    {

    }

    public override StateType DecideTransition()
    {
        if (GetDistanceToPlayer() < attackRadius)
            return StateType.Attack;

        return StateType.Idle;
    }

    public override void Execute()
    {
        movement.LookAt(playerTransform.position);
        gunController.OnTriggerHold();
    }

    float GetDistanceToPlayer()
    {
        return (playerTransform.position - transform.position).magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

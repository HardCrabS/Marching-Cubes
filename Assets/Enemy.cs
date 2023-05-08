using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Red,
    Yellow,
    Blue,
}


public class Enemy : Character
{
    public EnemyType enemyType;

    public override void TakeDamage()
    {
        EnemyNotification enemyNotification = new EnemyNotification(GetComponent<AttackState>(), transform.position, NotificationReason.TookDamage);
        EventsDispatcher.Instance.onNotifyEnemies?.Invoke(enemyNotification);
    }

    public override void Kill()
    {
        EventsDispatcher.Instance.onEnemyKilled?.Invoke(enemyType);
        base.Kill();
    }
}

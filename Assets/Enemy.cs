using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void TakeDamage()
    {
        EnemyNotification enemyNotification = new EnemyNotification(GetComponent<AttackState>(), transform.position, NotificationReason.TookDamage);
        EventsDispatcher.Instance.onNotifyEnemies?.Invoke(enemyNotification);
    }
}

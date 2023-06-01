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

    private void Start()
    {
        GunController gunController = GetComponent<GunController>();
        gunController.Initialize();
    }

    public override void TakeDamage()
    {
        EnemyNotification enemyNotification = new EnemyNotification(GetComponent<AttackState>(), transform.position, NotificationReason.TookDamage);
        EventsDispatcher.Instance.onNotifyEnemies?.Invoke(enemyNotification);
    }

    public override void Kill()
    {
        var gunCtrl = GetComponent<GunController>();
        gunCtrl.DropGun(gunCtrl.GetActiveGun().weaponData);
        EventsDispatcher.Instance.onEnemyKilled?.Invoke(enemyType);
        base.Kill();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    GunController gunController;

    public static Player Instance;

    public Action<int, int> onAmmoUpdated;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gunController = GetComponent<GunController>();
        EventsDispatcher.Instance.onInteract += gunController.EquipGun;
        EventsDispatcher.Instance.onTriggerHold += gunController.OnTriggerHold;
        EventsDispatcher.Instance.onReload += gunController.Reload;
        gunController.onGunSwitched += HandleSwitchGun;
    }

    void HandleSwitchGun(Gun gun)
    {
        gun.onAmmoUpdated += onAmmoUpdated;
    }
}

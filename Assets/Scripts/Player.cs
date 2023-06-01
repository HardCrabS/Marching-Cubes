using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player Instance;

    public Action<int, int> onAmmoUpdated;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        GunController gunController = GetComponent<GunController>();

        EventsDispatcher.Instance.onInteract += gunController.EquipGun;
        EventsDispatcher.Instance.onTriggerHold += gunController.OnTriggerHold;
        EventsDispatcher.Instance.onReload += gunController.Reload;
        gunController.onGunSwitched += HandleSwitchGun;

        gunController.Initialize();
    }

    void HandleSwitchGun(Gun gun)
    {
        var ammoInfo = gun.GetAmmoInfo();
        onAmmoUpdated?.Invoke(ammoInfo.Item1, ammoInfo.Item2);
        gun.onAmmoUpdated += onAmmoUpdated;
    }
}

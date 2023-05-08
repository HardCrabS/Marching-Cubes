using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    GunController gunController;

    private void Start()
    {
        gunController = GetComponent<GunController>();
        EventsDispatcher.Instance.onInteract += gunController.EquipGun;
        EventsDispatcher.Instance.onTriggerHold += gunController.OnTriggerHold;
        EventsDispatcher.Instance.onReload += gunController.Reload;
    }
}

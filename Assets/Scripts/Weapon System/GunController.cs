﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform gunHolder;
    public WeaponData[] weaponDatas;

    Gun equippedGun;
    WeaponData equippedWeaponData;

    private void Start()
    {
        EventsDispatcher.Instance.onInteract += EquipGun;
        if (weaponDatas.Length > 0)
        {
            EquipGun(weaponDatas[0]);
        }
    }

    void EquipGun(GameObject interactableGO)
    {
        var pickup = interactableGO.GetComponent<PickupGun>();
        if (!pickup)
            return;

        EquipGun(pickup.weaponData);
    }

    void EquipGun(WeaponData weaponToEquip)
    {
        if (equippedGun != null)
        {
            DropGun(equippedWeaponData);
            Destroy(equippedGun.gameObject);
        }
        equippedWeaponData = weaponToEquip;
        equippedGun = Instantiate(weaponToEquip.weaponPrefab, gunHolder.position, gunHolder.rotation, gunHolder);
        equippedGun.onPickUp.Invoke();
    }

    void DropGun(WeaponData weaponToDrop)
    {
        Instantiate(weaponToDrop.weaponPickupPrefab, gunHolder.position, gunHolder.rotation);
    }
}

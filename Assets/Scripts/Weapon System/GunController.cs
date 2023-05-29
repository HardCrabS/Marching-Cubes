using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform gunHolder;
    public WeaponData[] weaponsData;

    public Action<Gun> onGunSwitched;

    Gun activeGun;
    int activeSlot = 0;
    WeaponData[] equippedWeaponsData;

    const int EQUIPPED_WEAPONS_COUNT = 2;

    private void Start()
    {
        equippedWeaponsData = new WeaponData[EQUIPPED_WEAPONS_COUNT];
        for (int i = 0; i < EQUIPPED_WEAPONS_COUNT; i++)
        {
            if (i < weaponsData.Length && weaponsData[i])
                EquipGun(weaponsData[i], i);
        }
    }

    private void Update()
    {
        int scrollDiff = (int)Input.mouseScrollDelta.y;
        if (scrollDiff != 0)
        {
            int desiredSlot = (activeSlot + 1) % EQUIPPED_WEAPONS_COUNT;
            if (equippedWeaponsData[desiredSlot] != null)
            {
                activeSlot = desiredSlot;
                SwitchWeapon();
            }
        }
    }

    public void EquipGun(GameObject interactableGO)
    {
        var pickup = interactableGO.GetComponent<PickupGun>();
        if (!pickup)
            return;

        EquipGun(pickup.weaponData, GetFreeGunSlot());
    }

    void EquipGun(WeaponData weaponToEquip, int slot)
    {
        if (equippedWeaponsData[slot] != null)
        {
            DropGun(equippedWeaponsData[slot]);
            Destroy(activeGun.gameObject);
        }
        else
        {
            equippedWeaponsData[slot] = weaponToEquip;
            if (activeGun != null)
                return;
        }
        equippedWeaponsData[slot] = weaponToEquip;
        activeGun = Instantiate(weaponToEquip.weaponPrefab, gunHolder.position, gunHolder.rotation, gunHolder);
        activeGun.onPickUp.Invoke();
        onGunSwitched?.Invoke(activeGun);
    }

    void DropGun(WeaponData weaponToDrop)
    {
        Instantiate(weaponToDrop.weaponPickupPrefab, gunHolder.position, gunHolder.rotation);
    }

    void SwitchWeapon()
    {
        if (activeGun)
            Destroy(activeGun.gameObject);
        activeGun = Instantiate(equippedWeaponsData[activeSlot].weaponPrefab, gunHolder.position, gunHolder.rotation, gunHolder);
        activeGun.onPickUp.Invoke();
        onGunSwitched?.Invoke(activeGun);
    }

    int GetFreeGunSlot()
    {
        for (int i = 0; i < EQUIPPED_WEAPONS_COUNT; i++)
        {
            if (equippedWeaponsData[i] == null)
                return i;
        }
        return activeSlot;
    }

    public void OnTriggerHold()
    {
        if (activeGun != null)
        {
            activeGun.OnTriggerHold();
        }
    }

    public void Reload()
    {
        if (activeGun != null)
        {
            activeGun.Reload();
        }
    }
}

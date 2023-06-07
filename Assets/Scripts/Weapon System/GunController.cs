using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform gunHolder;
    public bool useEquippedDataFromFile = true;
    public WeaponData shovelWeaponData;
    public WeaponData[] weaponsData;

    public Action<Gun> onGunSwitched;

    Gun activeGun;
    int activeSlot = 0;
    WeaponData[] equippedWeaponsData;
    Transform autoAimTarget;
    bool isPlayer = false;

    const int EQUIPPED_WEAPONS_COUNT = GameConstants.EQUIPPED_WEAPONS_COUNT;

    public void Initialize(Transform autoAimTarget = null)
    {
        isPlayer = autoAimTarget == null;
        this.autoAimTarget = autoAimTarget;
        equippedWeaponsData = new WeaponData[EQUIPPED_WEAPONS_COUNT];
        if (useEquippedDataFromFile)
            weaponsData = LoadEquppedWeapons();
        for (int i = 0; i < EQUIPPED_WEAPONS_COUNT; i++)
        {
            if (i < weaponsData.Length && weaponsData[i])
                EquipGun(weaponsData[i], i);
        }
    }

    public void FinalizeCtrl()
    {
        if (activeGun)
        {
            Destroy(activeGun.GetComponentInChildren<BobAnimation>());
            Destroy(activeGun.GetComponentInChildren<WeaponSway>());
        }
    }

    public void UpdateWeapons()
    {
        if (activeGun)
        {
            Destroy(activeGun.gameObject);
            activeGun = null;
        }
        Initialize();
    }

    WeaponData[] LoadEquppedWeapons()
    {
        WeaponData[] weaponDatas = new WeaponData[EQUIPPED_WEAPONS_COUNT];
        string[] equipped = PlayerPrefsController.GetAllEquippedWeapons();
        if (equipped == null)
            return weaponDatas;
        Debug.Log("Equipped titles: ");
        for (int j = 0; j < equipped.Length; j++)
        {
            Debug.Log(equipped[j]);
        }
        int i = 0;
        foreach (var title in equipped)
        {
            string path = "Weapons/" + title;
            WeaponData weaponData = Resources.Load<WeaponData>(path);
            if (weaponData == null)
            {
                Debug.LogWarning("Weapon " + title + " not found.");
            }
            weaponDatas[i] = weaponData;
            i++;
        }

        return weaponDatas;
    }

    private void Update()
    {
        if (!isPlayer)
            return;

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
        if (autoAimTarget != null)
        {
            gunHolder.transform.LookAt(autoAimTarget);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Destroy(activeGun.gameObject);
            activeGun = Instantiate(shovelWeaponData.weaponPrefab, gunHolder.position, gunHolder.rotation, gunHolder);
            activeGun.onPickUp.Invoke();
            onGunSwitched?.Invoke(activeGun);
        }
    }

    public Gun GetActiveGun()
    {
        return activeGun;
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

    public void DropGun(WeaponData weaponToDrop)
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

    public void OnTriggerHold(MouseControl mouseControlMode = MouseControl.Shooting)
    {
        if (mouseControlMode == MouseControl.Shooting && activeGun != null)
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

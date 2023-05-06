using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform gunHolder;
    public WeaponData[] weaponsData;

    Gun equippedGun;
    WeaponData equippedWeaponData;

    private void Start()
    {
        if (weaponsData.Length > 0)
        {
            EquipGun(weaponsData[0]);
        }
    }

    public void EquipGun(GameObject interactableGO)
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

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void Reload()
    {
        if (equippedGun != null)
        {
            equippedGun.Reload();
        }
    }
}

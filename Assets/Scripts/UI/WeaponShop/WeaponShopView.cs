using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopView : MonoBehaviour
{
    public System.Action<WeaponButton> onWeaponEquipped;

    WeaponButton[] equippedWeaponButtons = new WeaponButton[GameConstants.EQUIPPED_WEAPONS_COUNT];
    int nextEquipIndex = 0;
    int equippedCount = 0;

    public void Init()
    {
        onWeaponEquipped += HandleEquipWeapon;
    }

    public void Fini()
    {
        onWeaponEquipped -= HandleEquipWeapon;
    }

    void HandleEquipWeapon(WeaponButton weaponButton)
    {
        if (equippedCount == GameConstants.EQUIPPED_WEAPONS_COUNT)
        {
            equippedWeaponButtons[nextEquipIndex].UnEquip();
            equippedCount--;
        }

        equippedWeaponButtons[nextEquipIndex] = weaponButton;
        nextEquipIndex = (nextEquipIndex + 1) % GameConstants.EQUIPPED_WEAPONS_COUNT;
        equippedCount++;
    }
}

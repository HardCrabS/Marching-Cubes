using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController
{
    const string MONEY_KEY = "Money";
    const string WEAPON_KEY = "Weapon";
    const string EQUIPPED_KEY = "Equipped";

    public static void SetMoney(int value)
    {
        PlayerPrefs.SetInt(MONEY_KEY, value);
    }
    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(MONEY_KEY, 0);
    }

    public static void UnlockWeapon(int id)
    {
        int unlockBits = PlayerPrefs.GetInt(WEAPON_KEY);
        unlockBits |= id;
        PlayerPrefs.SetInt(WEAPON_KEY, unlockBits);
    }
    public static bool IsWeaponUnlocked(int id)
    {
        return (PlayerPrefs.GetInt(WEAPON_KEY) & id) != 0;
    }

    public static void EquipWeapon(int id)
    {
        int equippedBits = PlayerPrefs.GetInt(EQUIPPED_KEY);
        equippedBits |= id;
        PlayerPrefs.SetInt(EQUIPPED_KEY, equippedBits);
    }
    public static bool IsWeaponEquipped(int id)
    {
        return (PlayerPrefs.GetInt(EQUIPPED_KEY) & id) != 0;
    }
    public static int GetEquippedMask()
    {
        return PlayerPrefs.GetInt(EQUIPPED_KEY);
    }
}
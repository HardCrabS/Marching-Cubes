using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController
{
    const string MONEY_KEY = "Money";
    const string WEAPON_KEY = "Weapon";
    const string EQUIPPED_KEY = "Equipped";
    const string EQUIPPED_TITLE_KEY = "EquippedTitle";
    const string UNLOCKED_ISLANDS_COUNT_KEY = "UnlockedIslandsCount";

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

    public static void EquipWeapon(int id, string title)
    {
        Debug.Log("Equipping weapon: " + title);
        int equippedBits = PlayerPrefs.GetInt(EQUIPPED_KEY);
        bool alreadyEquipped = (equippedBits & id) != 0;
        if (!alreadyEquipped)
        {
            EquipWeapon(title);
            equippedBits |= id;
            PlayerPrefs.SetInt(EQUIPPED_KEY, equippedBits);
        }
        else
            Debug.Log("Already equipped!");
    }
    public static void UnEquipWeapon(int id, string title)
    {
        int equippedBits = PlayerPrefs.GetInt(EQUIPPED_KEY);
        equippedBits &= ~id;
        PlayerPrefs.SetInt(EQUIPPED_KEY, equippedBits);
        UnEquipWeapon(title);
    }
    public static bool IsWeaponEquipped(int id)
    {
        return (PlayerPrefs.GetInt(EQUIPPED_KEY) & id) != 0;
    }
    public static int GetEquippedMask()
    {
        return PlayerPrefs.GetInt(EQUIPPED_KEY);
    }
    static void EquipWeapon(string key)
    {
        string equippedStr = PlayerPrefs.GetString(EQUIPPED_TITLE_KEY, "");
        if (!string.IsNullOrEmpty(equippedStr))
            equippedStr += " ";
        equippedStr += key;
        Debug.Log("Saving weapon title: " + key + ". Result string: " + equippedStr);
        PlayerPrefs.SetString(EQUIPPED_TITLE_KEY, equippedStr);
    }
    static void UnEquipWeapon(string key)
    {
        string equippedStr = PlayerPrefs.GetString(EQUIPPED_TITLE_KEY, "");
        if (string.IsNullOrEmpty(equippedStr))
            return;
        string[] equipped = equippedStr.Split(' ');
        string newEquippedStr = "";
        foreach (string title in equipped)
        {
            if (!title.StartsWith(key))
                newEquippedStr += title + " ";
        }
        newEquippedStr = newEquippedStr.Substring(0, newEquippedStr.Length - 1);
        Debug.Log($"Unequipped weapon {key}. New string: {newEquippedStr}");
        PlayerPrefs.SetString(EQUIPPED_TITLE_KEY, newEquippedStr);
    }
    public static string[] GetAllEquippedWeapons()
    {
        string equippedStr = PlayerPrefs.GetString(EQUIPPED_TITLE_KEY, "");
        Debug.Log("Loaded equpped string: " + equippedStr);
        if (string.IsNullOrEmpty(equippedStr))
            return null;
        string[] equpped = equippedStr.Split(' ');
        return equpped;
    }

    public static void UnlockIsland(string key)
    {
        PlayerPrefs.SetString(key, key);
        PlayerPrefs.SetInt(UNLOCKED_ISLANDS_COUNT_KEY, GetUnlockedIslandsCount() + 1);
    }
    public static bool IsIslandUnlocked(string key)
    {
        return !string.IsNullOrEmpty(PlayerPrefs.GetString(key, ""));
    }
    public static int GetUnlockedIslandsCount()
    {
        return PlayerPrefs.GetInt(UNLOCKED_ISLANDS_COUNT_KEY, 1);
    }
}
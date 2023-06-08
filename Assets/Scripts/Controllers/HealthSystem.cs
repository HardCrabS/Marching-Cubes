using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health = 100f;

    float curHealth;
    int curArmor = 0;

    private void Start()
    {
        curHealth = health;

        EventsDispatcher.Instance.onInteract += PickupHealth;

        if (GetComponent<Player>())
            LoadArmor();
    }

    public void PickupHealth(GameObject interactableGO)
    {
        var pickup = interactableGO.GetComponent<PickupHealth>();
        if (pickup == null)
            return;

        TakeDamage(-pickup.healthToAdd);
    }

    public HealthData GetHealthData()
    {
        HealthData hd = new HealthData();
        hd.healthInfo = new Tuple<float, float>(curHealth, health);
        hd.armorInfo = new Tuple<int, int>(curArmor, 100);
        return hd;
    }

    public void TakeDamage(float damage)
    {
        curArmor = (int)Mathf.Clamp(curArmor - damage, 0, curArmor);
        float damageAfterArmor = Mathf.Clamp(damage - curArmor, 0, damage);
        if (damageAfterArmor > 0)
        {
            curHealth = Mathf.Clamp(curHealth - damageAfterArmor, 0, health);
        }
        var character = GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage();
        }
        if (curHealth <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        EventsDispatcher.Instance.onInteract -= PickupHealth;

        var character = GetComponent<Character>();
        if (character != null)
        {
            character.Kill();
        }
    }

    void LoadArmor()
    {
        string armorTitle = PlayerPrefsController.GetEquippedArmor();
        if (string.IsNullOrEmpty(armorTitle))
            return;
        curArmor = Resources.Load<ArmorData>("Armor/" + armorTitle).health;

        PlayerPrefsController.EquipArmor("");
    }
}


public class HealthData
{
    // (curr, max)
    public Tuple<float, float> healthInfo;
    public Tuple<int, int> armorInfo;
}
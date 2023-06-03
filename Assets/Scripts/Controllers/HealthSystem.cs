using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health = 100f;

    float curHealth;

    private void Start()
    {
        curHealth = health;

        EventsDispatcher.Instance.onInteract += PickupHealth;
    }

    public void PickupHealth(GameObject interactableGO)
    {
        var pickup = interactableGO.GetComponent<PickupHealth>();
        if (pickup == null)
            return;

        TakeDamage(-pickup.healthToAdd);
    }

    public Tuple<float, float> GetHealthInfo()
    {
        return new Tuple<float, float>(curHealth, health);
    }

    public void TakeDamage(float damage)
    {
        curHealth = Mathf.Clamp(curHealth - damage, 0, health);
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
}

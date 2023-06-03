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
    }

    public Tuple<float, float> GetHealthInfo()
    {
        return new Tuple<float, float>(curHealth, health);
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
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
        var character = GetComponent<Character>();
        if (character != null)
        {
            character.Kill();
        }
    }
}

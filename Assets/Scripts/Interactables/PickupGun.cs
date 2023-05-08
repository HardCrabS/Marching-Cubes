using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGun : MonoBehaviour, IInteractable
{
    public WeaponData weaponData;

    public void Interact()
    {
        Destroy(gameObject);
    }
}

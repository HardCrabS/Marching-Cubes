using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour, IInteractable
{
    public float healthToAdd = 50;

    public void Interact()
    {
        Destroy(gameObject);
    }
}

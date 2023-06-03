using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTreasure : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
    }
}

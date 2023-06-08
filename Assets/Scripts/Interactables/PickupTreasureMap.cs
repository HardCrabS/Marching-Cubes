using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTreasureMap : MonoBehaviour, IInteractable
{
    void Start()
    {
        StatsController.Instance.IncrementTotalTreasures();

        Debug.Log("Treasure map spawned!");
    }

    public void Interact()
    {
        TreasureSpawner.Instance.SpawnTreasure();

        Destroy(gameObject);
    }
}

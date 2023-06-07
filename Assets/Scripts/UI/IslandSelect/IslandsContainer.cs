using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandsContainer : MonoBehaviour
{
    public void InitIslands()
    {
        Debug.Log("InitIslands");
        var islandButtons = GetComponentsInChildren<IslandButton>();
        foreach (var islandButton in islandButtons)
        {
            islandButton.Initialize();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSelectView : MonoBehaviour
{
    public Action<IslandViewData> onIslandClicked;
    public Action<IslandData> onIslandUnlocked;

    IslandInfoView islandInfoView;

    private void Start()
    {
        islandInfoView = GetComponentInChildren<IslandInfoView>();
        islandInfoView.gameObject.SetActive(false);

        onIslandClicked += (islandView) => { islandInfoView.gameObject.SetActive(true); };
        onIslandClicked += islandInfoView.SetView;
    }
}

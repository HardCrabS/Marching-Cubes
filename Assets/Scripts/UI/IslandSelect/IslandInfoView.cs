using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IslandViewData
{
    public IslandData islandData;
    public IslandButton islandButton;
    public bool isUnlocked;
}

public class IslandInfoView : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI enemyCountText;
    public Image enemyIconImage;
    public Button actionButton;

    IslandViewData selectedIsland;

    private void Start()
    {
        actionButton.onClick.AddListener(OnActionButtonClicked);
    }

    public void SetView(IslandViewData islandViewData)
    {
        selectedIsland = islandViewData;

        if (islandViewData.isUnlocked)
            SetIslandView(islandViewData);
        else
            SetLockedView(islandViewData);
    }

    void SetIslandView(IslandViewData islandViewData)
    {
        islandViewData.islandButton.UpdateButton(isHighlighted: true);
        titleText.text = islandViewData.islandData.title;
        descriptionText.text = islandViewData.islandData.description;
        rewardText.text = "reward: " + islandViewData.islandData.reward.ToString();
        enemyCountText.text = islandViewData.islandButton.Quest.amount.ToString() + "x";
        enemyIconImage.color = EnemyTypeToColor(islandViewData.islandButton.Quest.enemyPrefab.enemyType);
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "start";
    }

    void SetLockedView(IslandViewData islandViewData)
    {
        titleText.text = islandViewData.islandData.title;
        descriptionText.text = islandViewData.islandData.description;
        rewardText.text = "price: " + islandViewData.islandData.price.ToString();
        enemyCountText.text = "locked!";
        enemyIconImage.color = new Color(0,0,0,0);
        actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "buy";
    }

    void OnActionButtonClicked()
    {
        if (!selectedIsland.isUnlocked)
        {
            if (Buy())
            {
                SetView(selectedIsland);
                GetComponentInParent<IslandSelectView>().onIslandUnlocked?.Invoke(selectedIsland.islandData);
            }
        }
        else
        {
            QuestSystem.Instance.ActiveQuest = selectedIsland.islandButton.Quest;
            SceneManager.LoadScene(GameConstants.MAIN_GAME_SCENE_NAME);
        }
    }

    bool Buy()
    {
        if (Wallet.Instance.HasEnoughMoney(selectedIsland.islandData.price))
        {
            Wallet.Instance.AddMoney(-selectedIsland.islandData.price);
            PlayerPrefsController.UnlockIsland(selectedIsland.islandData.title);
            selectedIsland.isUnlocked = true;
            return true;
        }
        return false;
    }

    Color EnemyTypeToColor(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Red:
                return Color.red;
            case EnemyType.Blue:
                return Color.blue;
            case EnemyType.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsDisplay : MonoBehaviour
{
    public Text questText;

    void Start()
    {
        EventsDispatcher.Instance.onToggleMap += ShowQuestsInfo;
    }

    void ShowQuestsInfo()
    {
        Quest[] quests = QuestSystem.Instance.GetQuests();
        questText.text = $"{quests[0].amount} {quests[0].enemyPrefab.enemyType} enemies: {quests[0].progress}/{quests[0].amount}";
    }
}

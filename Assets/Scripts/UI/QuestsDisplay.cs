using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestsDisplay : MonoBehaviour
{
    public TextMeshProUGUI questText;

    void Start()
    {
        EventsDispatcher.Instance.onToggleMap += ShowQuestInfo;
    }

    void ShowQuestInfo()
    {
        if (!QuestSystem.Instance)
        {
            questText.text = "Quest system isn't found";
            return;
        }
        Quest quest = QuestSystem.Instance.ActiveQuest;
        questText.text = $"{quest.amount} {quest.enemyPrefab.enemyType} clones: {quest.progress}/{quest.amount}";
    }
}

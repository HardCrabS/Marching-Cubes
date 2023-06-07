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
        Quest quest = QuestSystem.Instance.ActiveQuest;
        questText.text = $"{quest.amount} {quest.enemyPrefab.enemyType} clones: {quest.progress}/{quest.amount}";
    }
}

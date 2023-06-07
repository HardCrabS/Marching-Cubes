using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour, IInteractable
{
    public Text finishText;

    bool isReadyToFinish = false;

    void Start()
    {
        EventsDispatcher.Instance.onQuestProgress += ProcessQuestProgress;

        finishText.text = "Complete all quests and come back.";
        finishText.color = Color.red;
    }

    public void Interact()
    {
        if (isReadyToFinish)
        {
            EventsDispatcher.Instance.onLevelFinished?.Invoke();
            finishText.text = "Success!";
            finishText.color = Color.yellow;
        }
    }

    void ProcessQuestProgress(Quest quest)
    {
        isReadyToFinish = QuestSystem.Instance.IsAllQuestsCompleted();
        if (isReadyToFinish)
        {
            finishText.text = "Ready to finish!\nPress E";
            finishText.color = Color.green;
        }
    }
}

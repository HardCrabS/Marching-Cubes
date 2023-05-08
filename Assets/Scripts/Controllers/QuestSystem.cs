using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest
{
    public Enemy enemyPrefab;
    public int amount;
    public int progress = 0;
    public bool completed = false;

    public Quest(Enemy prefab, int amount)
    {
        this.enemyPrefab = prefab;
        this.amount = amount;
    }
}


public class QuestSystem : MonoBehaviour
{
    public Vector2Int amountRange;
    public Enemy defaultEnemyPrefab;
    public Enemy[] enemyPrefabs;

    Quest[] quests;

    public static QuestSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        EventsDispatcher.Instance.onEnemyKilled += ProcessKilledEnemy;
    }

    public Quest[] GetQuests()
    {
        if (quests == null)
            GenerateQuests();
        return quests;
    }

    public bool IsAllQuestsCompleted()
    {
        if (quests == null)
            return false;
        foreach (var quest in quests)
        {
            if (!quest.completed)
                return false;
        }
        return true;
    }

    void GenerateQuests()
    {
        int randIndex = Random.Range(0, enemyPrefabs.Length);
        int randAmount = Random.Range(amountRange.x, amountRange.y);

        Quest quest = new Quest(enemyPrefabs[randIndex], randAmount);

        // TODO: add random amount of quests
        Quest[] quests = new Quest[1];
        quests[0] = quest;

        this.quests = quests;
    }

    void ProcessKilledEnemy(EnemyType enemyType)
    {
        foreach (var quest in quests)
        {
            if (!quest.completed && quest.enemyPrefab.enemyType == enemyType)
            {
                quest.progress++;
                if (quest.progress >= quest.amount)
                {
                    CompleteQuest(quest);
                }
                EventsDispatcher.Instance.onQuestProgress?.Invoke(quest);
                break;
            }
        }
    }

    void CompleteQuest(Quest quest)
    {
        quest.completed = true;
    }
}

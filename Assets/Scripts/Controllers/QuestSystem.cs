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

    public Quest ActiveQuest { get; set; }

    public static QuestSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(this);
    }

    public void StartListening()
    {
        EventsDispatcher.Instance.onEnemyKilled += ProcessKilledEnemy;
    }

    public bool IsAllQuestsCompleted()
    {
        return ActiveQuest.completed;
    }

    public Quest GenerateQuest()
    {
        int randIndex = Random.Range(0, enemyPrefabs.Length);
        int randAmount = Random.Range(amountRange.x, amountRange.y);

        Quest quest = new Quest(enemyPrefabs[randIndex], randAmount);
        return quest;
    }

    void ProcessKilledEnemy(EnemyType enemyType)
    {
        if (!ActiveQuest.completed && ActiveQuest.enemyPrefab.enemyType == enemyType)
        {
            ActiveQuest.progress++;
            if (ActiveQuest.progress >= ActiveQuest.amount)
            {
                CompleteQuest(ActiveQuest);
            }
            EventsDispatcher.Instance.onQuestProgress?.Invoke(ActiveQuest);
        }
    }

    void CompleteQuest(Quest quest)
    {
        quest.completed = true;
    }
}

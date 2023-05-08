using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public Transform chunksHolder;
    public bool isRandomAtStart = true;
    public float mapScaleFactor = 0.6f;
    public int maxEnemiesOnMap = 10;

    int enemiesSpawned = 0;

    void Start()
    {
        if (isRandomAtStart)
        {
            MapGenerator mapGen = FindObjectOfType<MapGenerator>();
            mapGen.seed = Random.Range(0, int.MaxValue);
        }
        if (MeshGenerator.Instance)
        {
            List<Chunk> chunks = MeshGenerator.Instance.InitChunks();
            chunksHolder.localScale *= mapScaleFactor;
            StartCoroutine(DelayedSpawn(chunks));
        }
    }

    IEnumerator DelayedSpawn(List<Chunk> chunks)
    {
        yield return new WaitForSeconds(1f);

        foreach (var chunk in chunks)
        {
            MeshGenerator.Instance.GeneratePropsOnChunk(chunk.transform);
        }

        EnemyBase[] enemyBases = FindObjectsOfType<EnemyBase>();
        SpawnQuestEnemies(enemyBases);
        SpawnDefaultEnemies(enemyBases);
    }

    void SpawnQuestEnemies(EnemyBase[] enemyBases)
    {
        Quest[] quests = QuestSystem.Instance.GetQuests();

        foreach (Quest quest in quests)
        {
            for (int i = 0; i < quest.amount; i++)
            {
                int randIndex = Random.Range(0, enemyBases.Length);
                EnemyBase enemyBase = enemyBases[randIndex];
                enemyBase.SpawnEnemy(quest.enemyPrefab);
                enemiesSpawned++;
            }
        }
    }

    void SpawnDefaultEnemies(EnemyBase[] enemyBases)
    {
        int leftToSpawn = maxEnemiesOnMap - enemiesSpawned;
        for (int i = 0; i < leftToSpawn; i++)
        {
            int randIndex = Random.Range(0, enemyBases.Length);
            EnemyBase enemyBase = enemyBases[randIndex];
            enemyBase.SpawnEnemy(QuestSystem.Instance.defaultEnemyPrefab);
            enemiesSpawned++;
        }
    }
}

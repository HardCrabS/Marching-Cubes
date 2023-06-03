using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public Transform chunksHolder;
    public bool isRandomAtStart = true;
    public float mapScaleFactor = 0.6f;
    public int maxEnemiesOnMap = 10;
    public GameObject levelFinishPrefab;

    public static IslandManager Instance;

    int enemiesSpawned = 0;

    List<Chunk> chunks;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (isRandomAtStart)
        {
            MapGenerator mapGen = FindObjectOfType<MapGenerator>();
            mapGen.seed = Random.Range(0, int.MaxValue);
        }
        if (MeshGenerator.Instance)
        {
            chunks = MeshGenerator.Instance.InitChunks();
            chunksHolder.localScale *= mapScaleFactor;
            FindObjectOfType<EndlessTerrain>().SetupChunks(chunks, mapScaleFactor);
            StartCoroutine(DelayedSpawn(chunks));
        }
    }

    public List<Chunk> GetChunks()
    {
        return chunks;
    }

    void MovePlayerOnEdge()
    {
        var meshGen = MeshGenerator.Instance;
        float mapSize = meshGen.MapSizeInWorldCoords;
        float roughRadius = mapSize * 0.9f / 2;

        Transform player = Player.Instance.transform;
        float randAngle = Random.Range(0, 360f);
        Vector3 randDirection = Quaternion.Euler(0, randAngle, 0) * Vector3.forward;
        Vector3 desiredPos = meshGen.MapCenter + randDirection * roughRadius;
        Physics.Raycast(desiredPos + Vector3.up * 100, Vector3.down, out RaycastHit hit, Mathf.Infinity);
        player.position = hit.point;
        Player.Instance.Initialize();

        // spawn finish level prop behind player
        desiredPos = meshGen.MapCenter + randDirection * roughRadius * 1.1f;
        Physics.Raycast(desiredPos + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity);
        Instantiate(levelFinishPrefab, hit.point, levelFinishPrefab.transform.rotation);
    }

    IEnumerator DelayedSpawn(List<Chunk> chunks)
    {
        // wait until terrain mesh collider is updated
        yield return new WaitForSeconds(1f);

        // spawn rare props at random chunk
        var randChunk = chunks[Random.Range(0, chunks.Count)];
        MeshGenerator.Instance.GeneratePropsOnChunk(randChunk.transform, rareProps: true);

        foreach (var chunk in chunks)
        {
            MeshGenerator.Instance.GeneratePropsOnChunk(chunk.transform);
        }

        EnemyBase[] enemyBases = FindObjectsOfType<EnemyBase>();
        SpawnQuestEnemies(enemyBases);
        SpawnDefaultEnemies(enemyBases);

        MovePlayerOnEdge();

        EventsDispatcher.Instance.onMapInitialized?.Invoke();
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

    private void OnDrawGizmos()
    {
        var meshGen = FindObjectOfType<MeshGenerator>();
        if (!meshGen)
            return;
        float mapSize = meshGen.ChunkSize * meshGen.chunksCount.x * meshGen.ChunksHolder.transform.localScale.x;
        Vector3 mapBottomLeft = meshGen.ChunksHolder.transform.position;
        Vector3 center = mapBottomLeft + new Vector3(1, 0, 1) * mapSize / 2;
        float roughRadius = mapSize * 0.9f / 2;
        Gizmos.DrawWireSphere(center, roughRadius);
    }
}

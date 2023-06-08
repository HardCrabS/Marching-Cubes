using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance;

    public int TotalEnemies { get; private set; }
    public int EnemiesKilled { get; private set; }

    public int TotalTreasures { get; private set; }
    public int TreasuresCollected { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventsDispatcher.Instance.onEnemyKilled += (eType) => { EnemiesKilled++; };
    }

    public void IncrementTotalEnemies()
    {
        TotalEnemies++;
    }
    public void IncrementTotalTreasures()
    {
        TotalTreasures++;
    }
    public void IncrementTreasuresCollected()
    {
        TreasuresCollected++;
    }
}

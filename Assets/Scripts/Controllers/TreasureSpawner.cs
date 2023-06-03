using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasurePrefab;
    public GameObject treasureCrossCanvas;

    [Space]
    public float depth = 2f;

    public static TreasureSpawner Instance;

    float minTreasureHeight = 6f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void SpawnTreasure()
    {
        var chunks = IslandManager.Instance.GetChunks();
        var chunkSize = MeshGenerator.Instance.ChunkSize;

        RaycastHit emptyHit = new RaycastHit();
        RaycastHit hit = emptyHit;
        while (hit.collider == null)
        {
            var randChunk = chunks[Random.Range(0, chunks.Count)];
            hit = PlacementGenerator.GetRandomHitAtChunk(randChunk.transform, chunkSize);
            if (hit.point.y < minTreasureHeight)
                hit = emptyHit;
        }

        var treasureCrossTransform = Instantiate(treasureCrossCanvas, hit.point, Quaternion.FromToRotation(-treasureCrossCanvas.transform.up, hit.normal)).transform;
        treasureCrossTransform.position = treasureCrossTransform.position + treasureCrossTransform.forward * 0.2f;

        Vector3 pos = treasureCrossTransform.position - treasureCrossTransform.forward * depth;
        var treasure = Instantiate(treasurePrefab, pos, Quaternion.identity);

        Minimap.Instance.DrawCrossOnPos(hit.point);
    }
}

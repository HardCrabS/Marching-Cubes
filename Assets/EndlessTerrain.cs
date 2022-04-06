using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public float maxViewDistance = 300;
    public Transform viewerTransform;

    public static Vector3 viewerPosition;

    int chunksVisibleInViewDst;

    Dictionary<Vector3Int, Chunk> terrainChunkDict = new Dictionary<Vector3Int, Chunk>();
    List<Chunk> terrainChunksVisibleLastUpdate = new List<Chunk>();

    void Start()
    {
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDistance / MeshGenerator.chunkSize);
    }

    void Update()
    {
        viewerPosition = new Vector3(viewerTransform.position.x, 0, viewerTransform.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currChunkCoordX = Mathf.FloorToInt(viewerPosition.x / MeshGenerator.chunkSize);
        int currChunkCoordZ = Mathf.FloorToInt(viewerPosition.z / MeshGenerator.chunkSize);
        Vector3Int currChunkCoord = new Vector3Int(currChunkCoordX, 0, currChunkCoordZ);

        for (int zOffset = -chunksVisibleInViewDst; zOffset <= chunksVisibleInViewDst; zOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector3Int viewedChunkCoord = currChunkCoord + new Vector3Int(xOffset, 0, zOffset);

                if(terrainChunkDict.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDict[viewedChunkCoord].UpdateTerrainChunk(viewerPosition, maxViewDistance);
                    if(terrainChunkDict[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDict[viewedChunkCoord]);
                    }
                }
                else
                {
                    Chunk createdChunk = MeshGenerator.Instance.CreateChunk(viewedChunkCoord);
                    terrainChunkDict[viewedChunkCoord] = createdChunk;
                }
            }
        }
    }
}

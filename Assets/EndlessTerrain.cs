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

                if (terrainChunkDict.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDict[viewedChunkCoord].UpdateTerrainChunk(viewerPosition, maxViewDistance);
                    if (terrainChunkDict[viewedChunkCoord].IsVisible())
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

    public void EditChunkPoints(Chunk chunk, Vector3 hitPos, float isolevelDiff, int terrainEditingRange)
    {
        Vector3 chunkPos = (Vector3)chunk.coord * MeshGenerator.chunkSize;
        Vector3Int pointPos = Vector3Int.RoundToInt((hitPos - chunkPos) / MeshGenerator.Instance.pointsOffset);

        Dictionary<Vector3Int, Chunk> affectedChunks = new Dictionary<Vector3Int, Chunk>();
        affectedChunks[chunk.coord] = chunk;

        for (int x = -terrainEditingRange; x <= terrainEditingRange; x++)
        {
            for (int y = -terrainEditingRange; y <= terrainEditingRange; y++)
            {
                for (int z = -terrainEditingRange; z <= terrainEditingRange; z++)
                {
                    Vector3Int offset = new Vector3Int(x, y, z);
                    if (offset.magnitude <= terrainEditingRange)
                    {
                        float isolevel = isolevelDiff;
                        if (terrainEditingRange > 0)
                        {
                            //interpolate isolevel, so the further point is from the origin -> the less it affects terrain
                            float t = 1 - offset.magnitude / terrainEditingRange;
                            isolevel = Mathf.Lerp(0, isolevelDiff, t);
                        }
                        Chunk ch = EditChunkPointRelativeTo(chunk, pointPos + offset, isolevel);
                        if (ch != null && !affectedChunks.ContainsKey(ch.coord))
                            affectedChunks[ch.coord] = ch;
                    }
                }
            }
        }

        foreach (Chunk ch in affectedChunks.Values)
        {
            MeshGenerator.Instance.UpdateChunkMesh(ch);
        }
    }

    //Identifying chunk based on provided point.
    //If point is outside of curr Chunk, then find the correct one and shift point position accordingly
    Chunk EditChunkPointRelativeTo(Chunk curr, Vector3Int point, float isolevelDiff)
    {
        int pointsPerAxis = MeshGenerator.Instance.pointsPerAxis;
        Vector3Int chunkOffset = Vector3Int.zero;

        //check if point is outside of curr chunk
        if (point.x < 0)
        {
            chunkOffset += Vector3Int.left;
            point.x += pointsPerAxis - 1;
        }
        else if (point.x >= pointsPerAxis)
        {
            chunkOffset += Vector3Int.right;
            point.x -= pointsPerAxis - 1;
        }
        if (point.z < 0)
        {
            chunkOffset += new Vector3Int(0, 0, -1);
            point.z += pointsPerAxis - 1;
        }
        else if (point.z >= pointsPerAxis)
        {
            chunkOffset += new Vector3Int(0, 0, 1);
            point.z -= pointsPerAxis - 1;
        }
        //check if we are on curr chunk at the border
        //if so, we also need to update adjacent border point at the neighbour chunk, since they are at the same position
        if (point.x == 0)
        {
            curr.UpdatePointIsolevel(point, isolevelDiff);
            return UpdatePointOnNeighbourChunk(curr.coord + Vector3Int.left, 
                                                  new Vector3Int(pointsPerAxis - 1, point.y, point.z), 
                                                  isolevelDiff);
        }
        else if(point.x == pointsPerAxis - 1)
        {
            curr.UpdatePointIsolevel(point, isolevelDiff);
            return UpdatePointOnNeighbourChunk(curr.coord + Vector3Int.right,
                                                  new Vector3Int(0, point.y, point.z),
                                                  isolevelDiff);
        }
        if(point.z == 0)
        {
            curr.UpdatePointIsolevel(point, isolevelDiff);
            return UpdatePointOnNeighbourChunk(curr.coord + new Vector3Int(0, 0, -1),
                                                  new Vector3Int(point.x, point.y, pointsPerAxis - 1),
                                                  isolevelDiff);
        }
        else if (point.z == pointsPerAxis - 1)
        {
            curr.UpdatePointIsolevel(point, isolevelDiff);
            return UpdatePointOnNeighbourChunk(curr.coord + new Vector3Int(0, 0, 1),
                                                  new Vector3Int(point.x, point.y, 0),
                                                  isolevelDiff);
        }

        Vector3Int chunkCoord = curr.coord + chunkOffset;
        return UpdatePointOnNeighbourChunk(chunkCoord, point, isolevelDiff);
    }

    Chunk UpdatePointOnNeighbourChunk(Vector3Int chunkCoord, Vector3Int point, float isolevelDiff)
    {
        if (terrainChunkDict.ContainsKey(chunkCoord))
        {
            Chunk foundChunk = terrainChunkDict[chunkCoord];
            foundChunk.UpdatePointIsolevel(point, isolevelDiff);
            return foundChunk;
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public float isolevel = 0;
    public bool hardEdges = false;
    public Vector3Int chunksCount;
    public Material terrainMat;

    [Header("Grid of points")]
    [Range(1, 100)]
    public int pointsPerAxis = 30;
    public float pointsOffset = 1;

    [Header("Gizmos")]
    public bool showGizmos = true;
    public Color colorGizmos;

    private List<Vector3> vertices;
    private List<int> triangles;

    private MapGenerator mapGen;
    private Point[,,] points;
    private GameObject chunksHolder;
    private const string chunksHolderName = "Chunks Holder";

    private void Awake()
    {
        //InitChunks();
    }

    void InitMesh(Mesh mesh)
    {
        // mesh is already inialized, so only clear mesh buffers
        if (mapGen)
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
            return;
        }
        mapGen = FindObjectOfType<MapGenerator>();

        vertices = new List<Vector3>();
        triangles = new List<int>();
    }
    void CreateChunksHolder()
    {
        if (chunksHolder == null)
        {
            GameObject findHolder = GameObject.Find(chunksHolderName);
            if (findHolder != null)
            {
                chunksHolder = findHolder;
            }
            else
            {
                chunksHolder = new GameObject(chunksHolderName);
            }
        }
    }
    public void InitChunks()
    {
        CreateChunksHolder();
        List<Chunk> oldChunks = new List<Chunk>(FindObjectsOfType<Chunk>());
        List<Chunk> newChunks = new List<Chunk>();

        // create chunks or edit existing ones
        for (int x = 0; x < chunksCount.x; x++)
        {
            for (int z = 0; z < chunksCount.z; z++)
            {
                Vector3Int chunk = new Vector3Int(x, 0, z);

                bool oldChunkFound = false;
                for (int i = 0; i < oldChunks.Count; i++)
                {
                    if(oldChunks[i].coord == chunk)
                    {
                        newChunks.Add(oldChunks[i]);
                        oldChunks.RemoveAt(i);
                        oldChunkFound = true;
                        break;
                    }
                }

                if(!oldChunkFound)
                {
                    newChunks.Add(CreateChunk(chunk));
                }
            }
        }

        for (int i = 0; i < oldChunks.Count; i++)
        {
            DestroyImmediate(oldChunks[i].gameObject, false);
        }

        for (int i = 0; i < newChunks.Count; i++)
        {
            Mesh mesh = GenerateMesh(newChunks[i].coord);
            Vector3 pos = (Vector3)newChunks[i].coord * (pointsPerAxis - 1) * pointsOffset;
            newChunks[i].transform.position = pos;
            newChunks[i].GetComponent<MeshFilter>().sharedMesh = mesh;
        }
    }

    Chunk CreateChunk(Vector3Int chunk)
    {
        Vector3 pos = (Vector3)chunk * (pointsPerAxis - 1) * pointsOffset;
        GameObject go = new GameObject("Terrain Mesh: " + chunk.ToString());
        go.transform.parent = chunksHolder.transform;
        go.transform.position = pos;

        Chunk chunkCo = go.AddComponent<Chunk>();
        chunkCo.coord = chunk;
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>().material = terrainMat;

        return chunkCo;
    }

    public Mesh GenerateMesh(Vector3Int chunk)
    {
        Mesh mesh = new Mesh();

        InitMesh(mesh);

        points = mapGen.GenerateMap(pointsPerAxis, pointsOffset, chunk);

        for (int x = 0; x < pointsPerAxis - 1; x++)
        {
            for (int y = 0; y < pointsPerAxis - 1; y++)
            {
                for (int z = 0; z < pointsPerAxis - 1; z++)
                {
                    March(new Vector3Int(x, y, z));
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    void March(Vector3Int id)
    {
        Point[] cornerCoords = {
            points[id.x, id.y, id.z],
            points[id.x+1, id.y, id.z],
            points[id.x+1, id.y, id.z+1],
            points[id.x, id.y, id.z+1],
            points[id.x, id.y+1, id.z],
            points[id.x+1, id.y+1, id.z],
            points[id.x+1, id.y+1, id.z+1],
            points[id.x, id.y+1, id.z+1],
        };

        int cubeConfiguration = 0;
        for (int i = 0; i < 8; i++)
        {
            if (cornerCoords[i].isolevel < isolevel)
            {
                cubeConfiguration |= (1 << i);
            }
        }

        int initVertexCount = vertices.Count;
        int[] edgeIndices = Table.GetEdgesArray(cubeConfiguration);

        // Create triangles for the current cube configuration
        for (int i = 0; edgeIndices[i] != -1; i += 3)
        {
            // Get indices of the two corner points defining the edge that the surface passes through.
            // (Do this for each of the three edges we're currently looking at).
            int edgeIndexA = edgeIndices[i];
            int a0 = Table.cornerIndexAFromEdge[edgeIndexA];
            int a1 = Table.cornerIndexBFromEdge[edgeIndexA];

            int edgeIndexB = edgeIndices[i + 1];
            int b0 = Table.cornerIndexAFromEdge[edgeIndexB];
            int b1 = Table.cornerIndexBFromEdge[edgeIndexB];

            int edgeIndexC = edgeIndices[i + 2];
            int c0 = Table.cornerIndexAFromEdge[edgeIndexC];
            int c1 = Table.cornerIndexBFromEdge[edgeIndexC];

            // Calculate positions of each vertex.
            Vector3 vertexA;
            Vector3 vertexB;
            Vector3 vertexC;
            if (hardEdges)
            {
                vertexA = VertexMiddle(cornerCoords[a0], cornerCoords[a1]);
                vertexB = VertexMiddle(cornerCoords[b0], cornerCoords[b1]);
                vertexC = VertexMiddle(cornerCoords[c0], cornerCoords[c1]);
            }
            else
            {
                vertexA = VertexInterp(cornerCoords[a0], cornerCoords[a1]);
                vertexB = VertexInterp(cornerCoords[b0], cornerCoords[b1]);
                vertexC = VertexInterp(cornerCoords[c0], cornerCoords[c1]);
            }

            // Create triangle
            vertices.Add(vertexA);
            vertices.Add(vertexB);
            vertices.Add(vertexC);

            triangles.Add(initVertexCount++);
            triangles.Add(initVertexCount++);
            triangles.Add(initVertexCount++);
        }
    }

    // interpolates based on point isolevel
    Vector3 VertexInterp(Point p1, Point p2)
    {
        float mu;
        Vector3 p;

        if (Mathf.Abs(isolevel - p1.isolevel) < 0.00001)
            return (p1.position);
        if (Mathf.Abs(isolevel - p2.isolevel) < 0.00001)
            return (p2.position);
        if (Mathf.Abs(p1.isolevel - p2.isolevel) < 0.00001)
            return (p1.position);
        mu = (isolevel - p1.isolevel) / (p2.isolevel - p1.isolevel);
        p.x = p1.position.x + mu * (p2.position.x - p1.position.x);
        p.y = p1.position.y + mu * (p2.position.y - p1.position.y);
        p.z = p1.position.z + mu * (p2.position.z - p1.position.z);

        return (p);
    }
    Vector3 VertexMiddle(Point p1, Point p2)
    {
        return (p1.position + p2.position) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = colorGizmos;
            Gizmos.DrawWireCube(Vector3.one * (pointsPerAxis - 1) * 0.5f * pointsOffset, Vector3.one * (pointsPerAxis - 1) * pointsOffset);
        }
    }
}

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

    public static float chunkSize = 0;

    private List<Vector3> vertices;
    private List<int> triangles;

    private MapGenerator mapGen;
    private const string chunksHolderName = "Chunks Holder";
    private GameObject chunksHolder;

    public static MeshGenerator Instance;

    public GameObject ChunksHolder
    {
        get
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
            return chunksHolder;
        }
        private set
        {
            chunksHolder = value;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        chunkSize = (pointsPerAxis - 1) * pointsOffset;
        //InitChunks();
    }

    void InitMesh(Mesh mesh)
    {
        // clear mesh buffers
        if (mapGen)
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
            return;
        }
        // initialize mesh generator data
        mapGen = FindObjectOfType<MapGenerator>();

        vertices = new List<Vector3>();
        triangles = new List<int>();
    }
    public void InitChunks()
    {
        //initialize for editor
        if(chunkSize == 0)
            chunkSize = (pointsPerAxis - 1) * pointsOffset;

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
                    if (oldChunks[i].coord == chunk)
                    {
                        newChunks.Add(oldChunks[i]);
                        oldChunks.RemoveAt(i);
                        oldChunkFound = true;
                        break;
                    }
                }

                if (!oldChunkFound)
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
            MeshData meshData = GenerateMesh(newChunks[i].coord);
            Vector3 pos = (Vector3)newChunks[i].coord * chunkSize;
            newChunks[i].transform.position = pos;
            newChunks[i].SetMesh(meshData.mesh, meshData.points);
        }
    }

    public Chunk CreateChunk(Vector3Int chunk)
    {
        GameObject go = new GameObject("Terrain Mesh: " + chunk.ToString());
        go.transform.parent = ChunksHolder.transform;

        Chunk chunkCo = go.AddComponent<Chunk>();
        MeshData meshData = GenerateMesh(chunk);
        chunkCo.SetUp(chunk, chunkSize, terrainMat);
        chunkCo.SetMesh(meshData.mesh, meshData.points);

        return chunkCo;
    }

    MeshData GenerateMesh(Vector3Int chunk)
    {
        Mesh mesh = new Mesh();

        InitMesh(mesh);

        Point[,,] points = mapGen.GenerateMap(pointsPerAxis, pointsOffset, chunk);

        for (int x = 0; x < pointsPerAxis - 1; x++)
        {
            for (int y = 0; y < pointsPerAxis - 1; y++)
            {
                for (int z = 0; z < pointsPerAxis - 1; z++)
                {
                    March(new Vector3Int(x, y, z), points);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return new MeshData(mesh, points);
    }
    public void UpdateChunkMesh(Chunk chunk)
    {
        Mesh mesh = new Mesh();

        InitMesh(mesh);

        Point[,,] points = chunk.GetPoints();

        for (int x = 0; x < pointsPerAxis - 1; x++)
        {
            for (int y = 0; y < pointsPerAxis - 1; y++)
            {
                for (int z = 0; z < pointsPerAxis - 1; z++)
                {
                    March(new Vector3Int(x, y, z), points);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        chunk.SetMesh(mesh);
    }

    void March(Vector3Int id, Point[,,] points)
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
            Gizmos.DrawWireCube(Vector3.one * chunkSize * 0.5f, Vector3.one * chunkSize);
        }
    }
}

class MeshData
{
    public Mesh mesh;
    public Point[,,] points;

    public MeshData(Mesh mesh, Point[,,] points)
    {
        this.mesh = mesh;
        this.points = points;
    }
}
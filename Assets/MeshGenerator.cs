using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    public float isolevel = 0;
    public bool hardEdges = false;

    List<Vector3> vertices;
    List<int> triangles;

    MapGenerator mapGen;
    Mesh mesh;

    private void Awake()
    {
        GenerateMesh();
    }

    void InitMesh()
    {
        // mesh is already inialized, so only clear mesh buffers
        if (mapGen)
        {
            mesh.Clear();
            vertices.Clear();
            triangles.Clear();
            return;
        }
        mapGen = GetComponent<MapGenerator>();

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "VoxelGrid Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void GenerateMesh()
    {
        InitMesh();

        mapGen.GenerateMap();

        for (int x = 0; x < mapGen.pointsPerAxis - 1; x++)
        {
            for (int y = 0; y < mapGen.pointsPerAxis - 1; y++)
            {
                for (int z = 0; z < mapGen.pointsPerAxis - 1; z++)
                {
                    March(new Vector3Int(x,y,z));
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void March(Vector3Int id)
    {
        Point[] cornerCoords = {
            mapGen.GetPoint(id.x, id.y, id.z),
            mapGen.GetPoint(id.x+1, id.y, id.z),
            mapGen.GetPoint(id.x+1, id.y, id.z+1),
            mapGen.GetPoint(id.x, id.y, id.z+1),
            mapGen.GetPoint(id.x, id.y+1, id.z),
            mapGen.GetPoint(id.x+1, id.y+1, id.z),
            mapGen.GetPoint(id.x+1, id.y+1, id.z+1),
            mapGen.GetPoint(id.x, id.y+1, id.z+1),
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
            if(hardEdges)
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
}

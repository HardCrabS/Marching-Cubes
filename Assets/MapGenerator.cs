using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point
{
    public Vector3 position;
    public float isolevel = 0;

    public Point(Vector3 _pos, float _isolevel)
    {
        position = _pos;
        isolevel = _isolevel;
    }
}

public class MapGenerator : MonoBehaviour
{
    [Header("Noise Map")]
    public float noiseScale = 0.3f;

    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2;

    public int seed;
    public Vector2 offset;
    public float noiseWeight = 5;
    public float floorOffset = 1;
    public float hardFloor = 1;
    public float hardFloorWeight = -1;
    public bool autoUpdate = true;

    [Header("Grid of points")]
    [Range(1, 100)]
    public int pointsPerAxis = 30;
    public float pointsOffset = 1;

    [Header("Gizmos")]
    public bool showGizmos = true;
    public Color colorGizmos;

    private Point[,,] points;

    public float[,] GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(pointsPerAxis, pointsPerAxis, seed, noiseScale, octaves, persistance, lacunarity, offset);
        GenerateGrid(noiseMap);

        return noiseMap;
    }

    public Point GetPoint(int x, int y, int z)
    {
        return points[x, y, z];
    }

    void GenerateGrid(float[,] noiseMap)
    {
        points = new Point[pointsPerAxis, pointsPerAxis, pointsPerAxis];

        for (int x = 0; x < points.GetLength(0); x++)
        {
            float xOffset = x * pointsOffset;
            for (int y = 0; y < points.GetLength(1); y++)
            {
                float yOffset = y * pointsOffset;
                for (int z = 0; z < points.GetLength(2); z++)
                {
                    float zOffset = z * pointsOffset;
                    Vector3 pos = new Vector3(xOffset, yOffset, zOffset);
                    float isolevel = GetIsolevel(pos, noiseMap[x, z]);
                    points[x, y, z] = new Point(pos, isolevel);
                }
            }
        }
    }
    float GetIsolevel(Vector3 pos, float noise)
    {
        float finalValue = (pos.y + floorOffset) + noise * noiseWeight;
        if (pos.y < hardFloor)
            finalValue += hardFloorWeight;
        return finalValue;
    }
    private void OnValidate()
    {
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 1)
        {
            octaves = 1;
        }
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

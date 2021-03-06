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
    [Header("Height Map")]
    public Noise.NormalizeMode normalizeMode;

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

    [Header("Heat Map")]
    public float heatNoiseScale = 10f;
    public float heatHeighInfluence = 0.8f;

    [Header("Moisture Map")]
    public float moistureNoiseScale = 10f;
    public float moistureHeighInfluence = 0.8f;

    [Space()]
    public bool autoUpdate = true;

    private float[,] heightMap;
    private float[,] heatMap;
    private float[,] moistureMap;

    public Point[,,] GenerateMap(int pointsPerAxis, float pointsOffset, Vector3Int chunk)
    {
        float chunkSize = pointsPerAxis - 1;
        Vector2 chunkOffset = new Vector2(chunk.x, chunk.z) * chunkSize;
        heightMap = Noise.GenerateNoiseMap(pointsPerAxis, pointsPerAxis, seed, noiseScale, octaves, 
                                          persistance, lacunarity, offset + chunkOffset, normalizeMode);
        heatMap = GenerateHeatMap(pointsPerAxis, chunkOffset);
        moistureMap = GenerateMoistureMap(pointsPerAxis, chunkOffset);
        return GenerateGrid(heightMap, pointsPerAxis, pointsOffset, chunk);
    }

    float[,] GenerateHeatMap(int pointsPerAxis, Vector2 chunkOffset)
    {
        float[,] heatNoiseMap = Noise.GenerateNoiseMap(pointsPerAxis, pointsPerAxis, seed, heatNoiseScale, 1,
                                          0.5f, 2f, offset + chunkOffset, normalizeMode);

        for (int x = 0; x < pointsPerAxis; x++)
        {
            for (int y = 0; y < pointsPerAxis; y++)
            {
                heatNoiseMap[x, y] -= heightMap[x, y] * heatHeighInfluence;
            }
        }

        return heatNoiseMap;
    }

    float[,] GenerateMoistureMap(int pointsPerAxis, Vector2 chunkOffset)
    {
        float[,] moistureNoiseMap = Noise.GenerateNoiseMap(pointsPerAxis, pointsPerAxis, seed, moistureNoiseScale, 1,
                                          0.5f, 2f, offset + chunkOffset, normalizeMode);

        for (int x = 0; x < pointsPerAxis; x++)
        {
            for (int y = 0; y < pointsPerAxis; y++)
            {
                moistureNoiseMap[x, y] -= heightMap[x, y] * moistureHeighInfluence;
            }
        }

        return moistureNoiseMap;
    }

    public float[,] GetHeightMap()
    {
        return heightMap;
    }
    public float[,] GetHeatMap()
    {
        return heatMap;
    }
    public float[,] GetMoistureMap()
    {
        return moistureMap;
    }
    Point[,,] GenerateGrid(float[,] noiseMap, int pointsPerAxis, float pointsOffset, Vector3Int chunk)
    {
        Point[,,] points = new Point[pointsPerAxis, pointsPerAxis, pointsPerAxis];

        for (int x = 0; x < points.GetLength(0); x++)
        {
            float xOffset = x * pointsOffset;
            for (int y = 0; y < points.GetLength(1); y++)
            {
                float yOffset = y * pointsOffset + chunk.y * MeshGenerator.chunkSize;
                for (int z = 0; z < points.GetLength(2); z++)
                {
                    float zOffset = z * pointsOffset;
                    Vector3 pos = new Vector3(xOffset, yOffset, zOffset);
                    float isolevel = GetIsolevel(pos, noiseMap[x, z]);
                    points[x, y, z] = new Point(pos, isolevel);
                }
            }
        }

        return points;
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
}

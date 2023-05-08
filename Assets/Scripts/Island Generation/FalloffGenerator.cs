using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size, Vector3Int chunk, int maxChunks, float a, float b)
    {
        float[,] map = new float[size, size];
        float chunkSize = size - 1;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = (i + chunkSize * chunk.x) / (chunkSize * maxChunks) * 2 - 1;
                float y = (j + chunkSize * chunk.z) / (chunkSize * maxChunks) * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value, a, b);
            }
        }

        return map;
    }
    
    static float Evaluate(float value, float a, float b)
    {
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b*value, a));
    }

    public static float[,] GenerateRadialMap(int size, Vector3Int chunk, int maxChunks)
    {
        float[,] map = new float[size, size];
        float chunkSize = size - 1;
        Vector2 maskCenter = 0.5f * chunkSize * maxChunks * Vector2.one;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float distFromCenter = Vector2.Distance(maskCenter, new Vector2(i + (chunkSize * chunk.x), j + (chunkSize * chunk.z)));
                map[i, j] = distFromCenter / size * maxChunks;
            }
        }

        return map;
    }
}

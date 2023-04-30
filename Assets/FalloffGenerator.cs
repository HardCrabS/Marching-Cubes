using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size, Vector3Int chunk, int maxChunks, float a, float b)
    {
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = (i + (float)size*chunk.x) / ((float)size * maxChunks) * 2 - 1;
                float y = (j + (float)size * chunk.z) / ((float)size * maxChunks) * 2 - 1;

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
}

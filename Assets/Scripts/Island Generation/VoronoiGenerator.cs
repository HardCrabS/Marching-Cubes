using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiGenerator : MonoBehaviour
{
    public int imageSize = 10;
    public float cellsPerRow = 5;

    public int noiseSize;
    public float noiseScale;
    public bool distort = true;
    public float distortOffset = 1f;

    //make private
    public Dictionary<Vector2, VoronoiPoint> voronoiPoints = new Dictionary<Vector2, VoronoiPoint>();

    //Generating grid of points spread on imageSize (width and height)
    void CreateVoronoiPoint(float cellSize, Vector2 voronoiPointPos)
    {
        Vector2 pos = new Vector2(voronoiPointPos.x, voronoiPointPos.y) * cellSize + (Vector2.one * cellSize / 2);
        float noiseValue = Mathf.PerlinNoise(pos.x, pos.y) * 2 - 1;
        Vector2 noiseOffset = new Vector2(noiseValue, noiseValue) * (cellSize / 2);
        Color col = new Color(Random.value, Random.value, Random.value);
        VoronoiPoint vorPoint = new VoronoiPoint(pos + noiseOffset, col);
        voronoiPoints[voronoiPointPos] = vorPoint;
    }

    public Texture2D GetVoronoiTexture(int textSize, float cellsPerRow, Vector3 chunkCoord, float chunkSize)
    {
        Vector3 chunkPosition = chunkCoord * chunkSize;
        float cellSize = chunkSize / cellsPerRow;
        Color[] colors = new Color[textSize * textSize];
        float[,] noiseX = Noise.GenerateNoiseMap(textSize, textSize, 1, 100, 4, 0.3f, 2, new Vector2(chunkPosition.x, chunkPosition.z), Noise.NormalizeMode.Global, true);
        float[,] noiseZ = Noise.GenerateNoiseMap(textSize, textSize, 2, 100, 4, 0.3f, 2, new Vector2(chunkPosition.x, chunkPosition.z), Noise.NormalizeMode.Global, true);
        float pixelSize = chunkSize / textSize;  // how texture size corresponds to chunk size
        for (int x = 0; x < textSize; x++)
        {
            for (int z = 0; z < textSize; z++)
            {
                Vector3 distortion = new Vector3(noiseX[x, z], 0, noiseZ[x, z]) * distortOffset;
                if (!distort)
                    distortion = Vector3.zero;

                VoronoiPoint closestPoint = GetClosestVoronoiPoint(chunkPosition + new Vector3(x, 0, z) * pixelSize + distortion, cellSize);
                //set color to the closest point's color
                colors[x * textSize + z] = closestPoint.color;
            }
        }

        Texture2D texture = new Texture2D(textSize, textSize);
        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

    VoronoiPoint GetClosestVoronoiPoint(Vector3 columnPosition, float cellSize)
    {
        //first find the closest not jittered point(as if all grid points are equally distant from each other)
        int indexX = Mathf.RoundToInt((columnPosition.x - cellSize / 2) / cellSize);
        int indexZ = Mathf.RoundToInt((columnPosition.z - cellSize / 2) / cellSize);

        if (!voronoiPoints.ContainsKey(new Vector2(indexX, indexZ)))
            CreateVoronoiPoint(cellSize, new Vector2(indexX, indexZ));

        //after that look for adjacent points to find the closest jittered one
        //it will always be in range [-1,1] since points jitter in range [-cellSize/2, cellSize/2]
        VoronoiPoint closestPoint = voronoiPoints[new Vector2(indexX, indexZ)];
        float minDist = float.MaxValue;
        for (int x = indexX - 1; x <= indexX + 1; x++)
        {
            for (int z = indexZ - 1; z <= indexZ + 1; z++)
            {
                if (!voronoiPoints.ContainsKey(new Vector2(x, z)))
                    CreateVoronoiPoint(cellSize, new Vector2(x, z));

                float dst = Vector2.Distance(columnPosition, voronoiPoints[new Vector2(x, z)].Vec3Pos);
                if (dst < minDist)
                {
                    closestPoint = voronoiPoints[new Vector2(x, z)];
                    minDist = dst;
                }
            }
        }
        return closestPoint;
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (var item in voronoiPoints.Values)
    //    {
    //        Gizmos.color = item.color;
    //        Gizmos.DrawSphere(item.Vec3Pos, 0.3f);
    //    }

    //    float pixelSize = FindObjectOfType<MeshGenerator>().ChunkSize / 50;
    //    for (int x = 0; x < 50; x++)
    //    {
    //        for (int z = 0; z < 50; z++)
    //        {
    //            Vector3 pos = new Vector3(x, 2, z) * pixelSize;
    //            Gizmos.DrawCube(pos, Vector3.one * 0.3f);
    //        }
    //    }
    //}
}

public class VoronoiPoint
{
    public Vector2 pos;
    public Color color;
    public Vector3 Vec3Pos { get { return new Vector3(pos.x, 0, pos.y); } }

    public VoronoiPoint(Vector2 pos, Color color)
    {
        this.pos = pos;
        this.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
            RegenerateMesh(mapGen);


        float[,] heightMap = mapGen.GetHeightMap();
        //float[,] heatMap = mapGen.GetHeatMap();
        //float[,] moistureMap = mapGen.GetMoistureMap();
        float[,] falloffMap = mapGen.GetFalloffMap();
        float[,] radialMap = mapGen.GetRadialMap();
        if (heightMap == null)
        {
            MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();
            if (meshGen)
            {
                mapGen.GenerateMap(meshGen.pointsPerAxis, meshGen.pointsOffset, new Vector3Int(0, 0, 0));
                heightMap = mapGen.GetHeightMap();
                //heatMap = mapGen.GetHeatMap();
                //moistureMap = mapGen.GetMoistureMap();
                falloffMap = mapGen.GetFalloffMap();
                radialMap = mapGen.GetRadialMap();
            }
        }


        Texture2D heightTexture = CreateNoiseTexture(heightMap, Color.black, Color.white);
        //Texture2D heatTexture = CreateNoiseTexture(heatMap, Color.blue, Color.red);
        //Texture2D moistureTexture = CreateNoiseTexture(moistureMap, Color.white, Color.blue);
        Texture2D falloffMapTexture = CreateNoiseTexture(falloffMap, Color.black, Color.white);
        Texture2D radialMapTexture = CreateNoiseTexture(radialMap, Color.black, Color.white);
        DrawTexture(heightTexture);
        DrawTexture(falloffMapTexture);
        DrawTexture(radialMapTexture);
        //DrawTexture(heatTexture);
        //DrawTexture(moistureTexture);
    }

    void DrawTexture(Texture2D texture)
    {
        GUILayout.Box("", GUILayout.Width(200), GUILayout.Height(200));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Vector2 pos = new Vector2(lastRect.x, lastRect.y);
        EditorGUI.DrawPreviewTexture(new Rect(pos, new Vector2(200, 200)), texture);
    }

    void RegenerateMesh(MapGenerator mapGen)
    {
        if(mapGen.autoUpdate)
        {
            MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();
            if(meshGen)
            {
                meshGen.InitChunks();
            }
        }
    }

    Texture2D CreateNoiseTexture(float[,] noiseMap, Color low, Color high)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(low, high, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();
        
        return texture;
    }
}

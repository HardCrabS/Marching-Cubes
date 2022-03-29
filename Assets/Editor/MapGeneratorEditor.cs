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

        Texture2D noiseTexture = CreateNoiseTexture(mapGen);
        GUILayout.Box("", GUILayout.Width(200), GUILayout.Height(200));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Vector2 pos = new Vector2(lastRect.x, lastRect.y);
        EditorGUI.DrawPreviewTexture(new Rect(pos, new Vector2(200, 200)), noiseTexture);
    }

    void RegenerateMesh(MapGenerator mapGen)
    {
        if(mapGen.autoUpdate)
        {
            MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();
            if(meshGen)
            {
                meshGen.GenerateMesh();
            }
        }
    }

    Texture2D CreateNoiseTexture(MapGenerator mapGen)
    {
        int width = mapGen.pointsPerAxis;
        int height = mapGen.pointsPerAxis;
        float[,] noiseMap = mapGen.GenerateMap();

        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();
        
        return texture;
    }
}

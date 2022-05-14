using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoronoiGenerator))]
public class VoronoiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var voronoi = target as VoronoiGenerator;

        if(GUILayout.Button("Generate"))
        {
            //voronoi.Generate();
            /*var texture =  voronoi.GetVoronoiTexture(voronoi.imageSize, voronoi.cellsPerRow, Vector2.zero, MeshGenerator.chunkSize);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            voronoi.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, 
                new Rect(0, 0, voronoi.imageSize, voronoi.imageSize), Vector2.one * 0.5f);*/

            voronoi.voronoiPoints.Clear();
            //FindObjectOfType<ColourGenerator>().UpdateShaderValues();
        }

        float[,] noiseX = Noise.GenerateNoiseMap(voronoi.noiseSize, voronoi.noiseSize, 0, voronoi.noiseScale,
                    4, 0.3f, 3.5f, Vector2.zero, Noise.NormalizeMode.Local, true);

        Texture2D text = CreateNoiseTexture(noiseX, Color.black, Color.white);
        DrawTexture(text);
    }

    void DrawTexture(Texture2D texture)
    {
        GUILayout.Box("", GUILayout.Width(200), GUILayout.Height(200));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Vector2 pos = new Vector2(lastRect.x, lastRect.y);
        EditorGUI.DrawPreviewTexture(new Rect(pos, new Vector2(200, 200)), texture);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColourGenerator : MonoBehaviour {
    public Material mat;
    public Gradient gradient;
    public float normalOffsetWeight;

    Texture2D texture;
    const int textureResolution = 50;

    void Init () {
        if (texture == null || texture.width != textureResolution) {
            texture = new Texture2D (textureResolution, 1, TextureFormat.RGBA32, false);
        }
    }

    public void Update () {
        Init ();
        UpdateTexture ();

        MeshGenerator m = FindObjectOfType<MeshGenerator>();

        //VoronoiGenerator voronoiGenerator = FindObjectOfType<VoronoiGenerator>();
        
        float boundsY = m.pointsPerAxis * m.pointsOffset;

        mat.SetFloat ("boundsY", boundsY);
        mat.SetFloat ("normalOffsetWeight", normalOffsetWeight);
        mat.SetTexture ("ramp", texture);
        //var chunks = FindObjectsOfType<Chunk>();
        //foreach (var ch in chunks)
        //{

        //    Texture2D voronoiTexture = voronoiGenerator.GetVoronoiTexture(500, voronoiGenerator.cellsPerRow, ch.coord, m.ChunkSize);
        //    var go = new GameObject("sprite").AddComponent<SpriteRenderer>().sprite =
        //    Sprite.Create(voronoiTexture, new Rect(0, 0, textureResolution, textureResolution), Vector2.one * 0.5f);
        //    ch.GetComponent<Renderer>().sharedMaterial.SetTexture("moistureTexture", voronoiTexture);
        //}
    }

    void UpdateTexture () {
        if (gradient != null) {
            Color[] colours = new Color[texture.width];
            for (int i = 0; i < textureResolution; i++) {
                Color gradientCol = gradient.Evaluate (i / (textureResolution - 1f));
                colours[i] = gradientCol;
            }

            texture.SetPixels (colours);
            texture.Apply ();
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
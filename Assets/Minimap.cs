using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Camera minimapCamera;
    public Vector2Int textureSize = new Vector2Int(100, 100);

    Image minimapImage;

    private void Start()
    {
        EventsDispatcher.Instance.onToggleMap += ToggleMinimap;
        EventsDispatcher.Instance.onMapInitialized += () =>
        {
            if (!minimapCamera)
                return;
            Sprite sprite = GenerateMinimap();
            minimapImage.sprite = sprite;
        };

        minimapImage = GetComponentInChildren<Image>();
        ToggleMinimap();
    }

    void ToggleMinimap()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        } 
    }

    Sprite GenerateMinimap()
    {
        int width = textureSize.x, height = textureSize.y;

        RenderTexture renderTexture = new RenderTexture(width, height, 1);
        minimapCamera.targetTexture = renderTexture;
        minimapCamera.Render();

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        DrawPlayerStartPosition(texture);

        texture.Apply();
        RenderTexture.active = null;

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
    }

    private void DrawPlayerStartPosition(Texture2D texture)
    {
        float mapSizeInWorldCoords = MeshGenerator.Instance.MapSizeInWorldCoords;
        Vector3 playerStartPos = FindObjectOfType<Player>().transform.position;
        int startPosX = (int)(playerStartPos.x / mapSizeInWorldCoords * textureSize.x);
        int startPosY = (int)(playerStartPos.z / mapSizeInWorldCoords * textureSize.y);
        Vector2Int playerStartPosOnMinimap = new Vector2Int(startPosX, startPosY);
        DrawSquare(texture, playerStartPosOnMinimap.x, playerStartPosOnMinimap.y, 5);
    }

    void DrawSquare(Texture2D texture, int xStart, int yStart, int thickness)
    {
        for (int x = -thickness; x <= thickness; x++)
        {
            for (int y = -thickness; y <= thickness; y++)
            {
                int targetX = Mathf.Clamp(xStart + x, 0, textureSize.x);
                int targetY = Mathf.Clamp(yStart + y, 0, textureSize.y);
                texture.SetPixel(targetX, targetY, Color.red);
            }
        }
    }
}

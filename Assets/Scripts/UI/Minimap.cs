using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Camera minimapCamera;
    public Vector2Int textureSize = new Vector2Int(100, 100);
    public GameObject blur;

    public static Minimap Instance;

    Image minimapImage;

    private void Awake()
    {
        Instance = this;
    }

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

    public void DrawCrossOnPos(Vector3 pos)
    {
        var texture = minimapImage.sprite.texture;
        Texture2D modifiedTexture = new Texture2D(texture.width, texture.height);
        modifiedTexture.SetPixels(texture.GetPixels());

        float mapSizeInWorldCoords = MeshGenerator.Instance.MapSizeInWorldCoords;
        int startPosX = (int)(pos.x / mapSizeInWorldCoords * textureSize.x);
        int startPosY = (int)(pos.z / mapSizeInWorldCoords * textureSize.y);
        Vector2Int posOnMinimap = new Vector2Int(startPosX, startPosY);

        DrawCross(modifiedTexture, posOnMinimap.x, posOnMinimap.y, 8, 3);

        modifiedTexture.Apply();

        minimapImage.sprite = Sprite.Create(modifiedTexture, minimapImage.sprite.rect, minimapImage.sprite.pivot);
    }

    void ToggleMinimap()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
        blur.SetActive(!blur.activeSelf);
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
        Vector3 playerStartPos = Player.Instance.transform.position;
        int startPosX = (int)(playerStartPos.x / mapSizeInWorldCoords * textureSize.x);
        int startPosY = (int)(playerStartPos.z / mapSizeInWorldCoords * textureSize.y);
        Vector2Int playerStartPosOnMinimap = new Vector2Int(startPosX, startPosY);
        DrawSquare(texture, playerStartPosOnMinimap.x, playerStartPosOnMinimap.y, 5);
    }

    void DrawSquare(Texture2D texture, int xCenter, int yCenter, int thickness)
    {
        for (int x = -thickness; x <= thickness; x++)
        {
            for (int y = -thickness; y <= thickness; y++)
            {
                int targetX = Mathf.Clamp(xCenter + x, 0, textureSize.x);
                int targetY = Mathf.Clamp(yCenter + y, 0, textureSize.y);
                texture.SetPixel(targetX, targetY, Color.red);
            }
        }
    }

    void DrawCross(Texture2D texture, int xCenter, int yCenter, int size, int thickness)
    {
        for (int i = -size; i <= size; i++)
        {
            int targetX = Mathf.Clamp(xCenter + i, 0, textureSize.x);
            int targetY = Mathf.Clamp(yCenter + i, 0, textureSize.y);
            texture.SetPixel(targetX, targetY, Color.red);

            for (int j = 0; j < thickness; j++)
            {
                texture.SetPixel(targetX + j, targetY, Color.red);
                texture.SetPixel(targetX, targetY + j, Color.red);
            }

            targetX = Mathf.Clamp(xCenter - i, 0, textureSize.x);
            texture.SetPixel(targetX, targetY, Color.red);

            for (int j = 0; j < thickness; j++)
            {
                texture.SetPixel(targetX - j, targetY, Color.red);
                texture.SetPixel(targetX, targetY + j, Color.red);
            }
        }
    }
}

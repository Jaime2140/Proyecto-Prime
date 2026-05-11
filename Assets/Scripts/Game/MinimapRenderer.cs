using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer : MonoBehaviour
{
    [Header("Referencias")]
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;
    
    [Header("UI Components")]
    public RectTransform mapContainer;
    public RawImage mapImage;
    
    [Header("Configuración Visual")]
    public int pixelsPerTile = 10;

    [Header("Colores del Mapa")]
    public Color floorColor = Color.white;
    public Color wallColor = Color.black;
    public Color streetColor = Color.gray;
    public Color buildingColor = Color.red;
    public Color parkColor = Color.green;

    private Texture2D texture;

    void Start()
    {
        GenerateMapTexture();
    }

    void LateUpdate()
    {
        UpdateMapPosition();
    }

    void GenerateMapTexture()
    {
        if (dungeonMap == null) return;

        int width = dungeonMap.Width;
        int height = dungeonMap.Height;

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileType tile = dungeonMap.GetTile(x, y);
                texture.SetPixel(x, y, GetColorForTile(tile));
            }
        }
        texture.Apply();
        mapImage.texture = texture;

        mapImage.rectTransform.sizeDelta = new Vector2(width * pixelsPerTile, height * pixelsPerTile);
        
        mapImage.rectTransform.pivot = Vector2.zero;
    }

    void UpdateMapPosition()
    {
        if (player == null || mapContainer == null) return;

        float playerMapX = (player.position.x + 0.5f) * pixelsPerTile;
        float playerMapY = (player.position.y + 0.5f) * pixelsPerTile;

        float windowCenterX = mapContainer.rect.width / 2f;
        float windowCenterY = mapContainer.rect.height / 2f;

        Vector2 targetPos = new Vector2(windowCenterX - playerMapX, windowCenterY - playerMapY);

        mapImage.rectTransform.anchoredPosition = targetPos;
    }

    Color GetColorForTile(TileType tile)
    {
        switch (tile)
        {
            case TileType.Empty: return Color.clear;
            case TileType.Wall:     return wallColor;
            case TileType.Building: return buildingColor;
            case TileType.Street:   return streetColor;
            case TileType.Park:     return parkColor;
            case TileType.Floor:    return floorColor;
            default:                return Color.magenta;
        }
    }
}
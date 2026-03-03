using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer : MonoBehaviour
{
    [Header("Referencias")]
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;
    
    [Header("UI Components")]
    public RectTransform mapContainer; // El objeto padre de 125x86 (La ventana)
    public RawImage mapImage;          // El RawImage hijo (El mapa gigante)
    
    [Header("Configuración Visual")]
    public int pixelsPerTile = 10; // ¡Zoom! Cuantos píxeles de UI mide una casilla

    [Header("Colores del Mapa")]
    public Color wallColor = Color.black;
    public Color floorColor = Color.gray;
    public Color streetColor = Color.gray;
    public Color buildingColor = Color.darkGray;
    public Color parkColor = new Color(0, 0.5f, 0); // Verde oscuro

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

        // 1. Generar la textura (Igual que antes)
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

        // 2. IMPORTANTE: Ajustar el tamaño real del RawImage según el Zoom
        // Si el mapa es de 20x20 y cada tile son 10px, la imagen medirá 200x200
        mapImage.rectTransform.sizeDelta = new Vector2(width * pixelsPerTile, height * pixelsPerTile);
        
        // Aseguramos que el Pivot esté en 0,0 para que las matemáticas funcionen fácil
        mapImage.rectTransform.pivot = Vector2.zero;
    }

    void UpdateMapPosition()
    {
        if (player == null || mapContainer == null) return;

        // 1. Dónde está el jugador en coordenadas del mapa (Píxeles UI)
        // El +0.5f es para centrar en la casilla
        float playerMapX = (player.position.x + 0.5f) * pixelsPerTile;
        float playerMapY = (player.position.y + 0.5f) * pixelsPerTile;

        // 2. Calculamos el centro de nuestra ventana (visor)
        float windowCenterX = mapContainer.rect.width / 2f;
        float windowCenterY = mapContainer.rect.height / 2f;

        // 3. Movemos el MAPA en dirección contraria para que el punto del jugador 
        // coincida con el centro de la ventana.
        // Fórmula: PosiciónMapa = CentroVentana - PosiciónJugadorEnMapa
        Vector2 targetPos = new Vector2(windowCenterX - playerMapX, windowCenterY - playerMapY);

        mapImage.rectTransform.anchoredPosition = targetPos;
    }

    Color GetColorForTile(TileType tile)
    {
        switch (tile)
        {
            case TileType.Wall:     return wallColor;
            case TileType.Building: return buildingColor;
            case TileType.Street:   return streetColor;
            case TileType.Park:     return parkColor;
            case TileType.Floor:    return floorColor;
            default:                return Color.magenta;
        }
    }
}
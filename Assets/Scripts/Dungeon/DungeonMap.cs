using UnityEngine;

public enum TileType
{
    Floor,
    Wall,
    Street,
    Building,
    Park,
    Empty
}

public class DungeonMap : MonoBehaviour
{
    [Header("Archivo de Mapa")]
    [Tooltip("Arrastra aquí tu imagen .png o .jpg (debe ser Read/Write Enabled)")]
    public Texture2D mapImage; 

    [Header("Diccionario de Colores")]
    public Color colorFloor    = Color.white;
    public Color colorWall     = Color.black;
    public Color colorStreet   = Color.gray;
    public Color colorBuilding = Color.red;
    public Color colorPark     = Color.green;

    private TileType[,] map;

    public int Width  => map != null ? map.GetLength(1) : 0;
    public int Height => map != null ? map.GetLength(0) : 0;

    void Awake()
    {
        if (mapImage != null)
        {
            GenerateMapFromImage();
        }
        else
        {
            Debug.LogError("ERROR CRÍTICO: ¡No has asignado la imagen 'mapImage' en el inspector de DungeonMap!");
            map = new TileType[1, 1]; 
        }
    }

    void GenerateMapFromImage()
    {
        int w = mapImage.width;
        int h = mapImage.height;
        
        map = new TileType[h, w];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Color pixelColor = mapImage.GetPixel(x, y);
                
                map[y, x] = ColorToTile(pixelColor);
            }
        }
        
        Debug.Log($"Mapa generado con éxito: {w}x{h}");
    }

    TileType ColorToTile(Color current)
    {
        if (current.a < 0.1f) return TileType.Empty;
        if (IsSameColor(current, colorWall))     return TileType.Wall;
        if (IsSameColor(current, colorFloor))    return TileType.Floor;
        if (IsSameColor(current, colorStreet))   return TileType.Street;
        if (IsSameColor(current, colorBuilding)) return TileType.Building;
        if (IsSameColor(current, colorPark))     return TileType.Park;

        return TileType.Wall; 
    }

    bool IsSameColor(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.1f &&
               Mathf.Abs(a.g - b.g) < 0.1f &&
               Mathf.Abs(a.b - b.b) < 0.1f;
    }
    
    public bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public TileType GetTile(int x, int y)
    {
        if (!InBounds(x, y)) return TileType.Wall; 
        return map[y, x];
    }

    public bool IsWall(int x, int y) => GetTile(x, y) == TileType.Wall;
    public bool IsStreet(int x, int y) => GetTile(x, y) == TileType.Street;
    public bool IsBuilding(int x, int y) => GetTile(x, y) == TileType.Building;
    public bool IsPark(int x, int y) => GetTile(x, y) == TileType.Park;

    public bool BlocksView(int x, int y)
    {
        TileType t = GetTile(x, y);
        return t == TileType.Wall || t == TileType.Building;
    }

    public bool BlocksMovement(int x, int y)
    {
        TileType t = GetTile(x, y);
        return t == TileType.Wall || t == TileType.Building || t == TileType.Empty;
    }

    public bool IsFloor(int x, int y)
    {
        TileType t = GetTile(x, y);
        return t == TileType.Floor || t == TileType.Street || t == TileType.Park;
    }
}
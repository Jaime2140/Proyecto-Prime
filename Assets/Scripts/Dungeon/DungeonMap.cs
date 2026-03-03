using UnityEngine;

// Mantenemos tu Enum igual
public enum TileType
{
    Floor,      // Blanco (por ejemplo)
    Wall,       // Negro
    Street,     // Gris
    Building,   // Rojo
    Park        // Verde
}

public class DungeonMap : MonoBehaviour
{
    [Header("Archivo de Mapa")]
    [Tooltip("Arrastra aquí tu imagen .png o .jpg (debe ser Read/Write Enabled)")]
    public Texture2D mapImage; 

    [Header("Diccionario de Colores")]
    // Define qué color representa qué cosa. Puedes cambiarlos en el inspector.
    public Color colorFloor    = Color.white;  // 255, 255, 255 rgb
    public Color colorWall     = Color.black;  // 0, 0, 0 rgb
    public Color colorStreet   = Color.gray;   // 128, 128, 128 rgb
    public Color colorBuilding = Color.red;    // 255, 0, 0 rgb
    public Color colorPark     = Color.green;  // 0, 255, 0 rgb

    // La matriz ya no es fija, se generará sola
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
            // Creamos un mapa de seguridad de 1x1 para que el juego no crashee
            map = new TileType[1, 1]; 
        }
    }

    void GenerateMapFromImage()
    {
        int w = mapImage.width;
        int h = mapImage.height;
        
        // Inicializamos la matriz con el tamaño de la imagen
        map = new TileType[h, w];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                // Leemos el color del píxel (x, y)
                Color pixelColor = mapImage.GetPixel(x, y);
                
                // Lo convertimos a TileType y lo guardamos
                map[y, x] = ColorToTile(pixelColor);
            }
        }
        
        Debug.Log($"Mapa generado con éxito: {w}x{h}");
    }

    TileType ColorToTile(Color current)
    {
        // Comparamos colores. Usamos una pequeña función auxiliar para evitar errores de redondeo.
        if (IsSameColor(current, colorWall))     return TileType.Wall;
        if (IsSameColor(current, colorFloor))    return TileType.Floor;
        if (IsSameColor(current, colorStreet))   return TileType.Street;
        if (IsSameColor(current, colorBuilding)) return TileType.Building;
        if (IsSameColor(current, colorPark))     return TileType.Park;

        // Si el color no se reconoce, por seguridad lo hacemos Pared
        return TileType.Wall; 
    }

    bool IsSameColor(Color a, Color b)
    {
        // Tolerancia pequeña (0.1) por si la compresión de imagen altera levemente el color
        return Mathf.Abs(a.r - b.r) < 0.1f &&
               Mathf.Abs(a.g - b.g) < 0.1f &&
               Mathf.Abs(a.b - b.b) < 0.1f;
    }

    // ---------------------------------------------------------
    // TUS MÉTODOS ORIGINALES (Se mantienen igual para compatibilidad)
    // ---------------------------------------------------------
    
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
        return t == TileType.Wall || t == TileType.Building;
    }

    public bool IsFloor(int x, int y)
    {
        TileType t = GetTile(x, y);
        return t == TileType.Floor || t == TileType.Street || t == TileType.Park;
    }
}
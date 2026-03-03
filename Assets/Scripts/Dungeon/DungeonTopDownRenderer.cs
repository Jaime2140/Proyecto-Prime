using UnityEngine;

public class DungeonTopDownRenderer : MonoBehaviour
{
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;

    public int cellSize = 1;

    [Header("Colores por TileType")]
    public Color floorColor = Color.white;
    public Color wallColor = Color.black;
    public Color buildingColor = new Color(0.3f, 0.3f, 0.3f);
    public Color streetColor = new Color(0.7f, 0.7f, 0.7f);
    public Color parkColor = Color.green;

    public Color playerColor = Color.magenta;

    void OnDrawGizmos()
    {
        if (dungeonMap == null)
            return;

        for (int x = 0; x < dungeonMap.Width; x++)
        {
            for (int y = 0; y < dungeonMap.Height; y++)
            {
                TileType tile = dungeonMap.GetTile(x, y);

                Gizmos.color = GetColorForTile(tile);

                Vector3 pos = new Vector3(
                    x * cellSize,
                    y * cellSize,
                    0
                );

                Gizmos.DrawCube(pos, Vector3.one * cellSize);
            }
        }

        // Dibujar jugador
        if (player != null)
        {
            Gizmos.color = playerColor;
            Vector3 playerPos = new Vector3(
                player.position.x * cellSize,
                player.position.y * cellSize,
                0
            );
            Gizmos.DrawCube(playerPos, Vector3.one * cellSize * 0.6f);
        }
    }

    Color GetColorForTile(TileType tile)
    {
        switch (tile)
        {
            case TileType.Wall:
                return wallColor;

            case TileType.Building:
                return buildingColor;

            case TileType.Street:
                return streetColor;

            case TileType.Park:
                return parkColor;

            case TileType.Floor:
            default:
                return floorColor;
        }
    }
}

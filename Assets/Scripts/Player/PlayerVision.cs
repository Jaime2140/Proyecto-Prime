using UnityEngine;
using System.Collections.Generic;

public class PlayerVision : MonoBehaviour
{
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;

    public int viewDistance = 3;

    public List<Vector2Int> GetVisibleCells()
    {
        List<Vector2Int> visible = new List<Vector2Int>();

        Vector2Int forward = DirectionToVector(player.facing);
        Vector2Int left    = new Vector2Int(-forward.y, forward.x);
        Vector2Int right   = new Vector2Int(forward.y, -forward.x);

        Vector2Int origin = player.position;

        for (int d = 1; d <= viewDistance; d++)
        {
            Vector2Int center = origin + forward * d;

            visible.Add(center);
            visible.Add(center + left);
            visible.Add(center + right);

            if (dungeonMap.IsWall(center.x, center.y))
                break;
        }

        return visible;
    }

    Vector2Int DirectionToVector(PlayerGridMovement.Direction dir)
    {
        switch (dir)
        {
            case PlayerGridMovement.Direction.North: return Vector2Int.up;
            case PlayerGridMovement.Direction.South: return Vector2Int.down;
            case PlayerGridMovement.Direction.East:  return Vector2Int.right;
            case PlayerGridMovement.Direction.West:  return Vector2Int.left;
        }
        return Vector2Int.zero;
    }
    void OnDrawGizmos()
    {
        if (dungeonMap == null || player == null)
            return;

        Gizmos.color = Color.cyan;

        foreach (var cell in GetVisibleCells())
        {
            Gizmos.DrawWireCube(
                new Vector3(cell.x, cell.y, 0),
                Vector3.one
            );
        }
    }

}

using UnityEngine;

public class DungeonGlobalEnvironmentRenderer : MonoBehaviour
{
    [Header("Referencias Lógicas")]
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;

    [Header("Sprites Globales")]
    public SpriteRenderer sky;       
    public SpriteRenderer floor;     
    public SpriteRenderer leftSide;
    public SpriteRenderer rightSide;
    public SpriteRenderer backGround; 

    [Header("Opciones de Sorting")]
    public int skySortingOrder       = -1000;
    public int floorSortingOrder     = 0;
    public int sideSortingOrder      = 50;
    public int backgroundSortingOrder = -100;

    void Awake()
    {
        if (sky != null) sky.sortingOrder = skySortingOrder;
        if (floor != null) floor.sortingOrder = floorSortingOrder;
        if (leftSide != null) leftSide.sortingOrder = sideSortingOrder;
        if (rightSide != null) rightSide.sortingOrder = sideSortingOrder;
        if (backGround != null) backGround.sortingOrder = backgroundSortingOrder;
    }

    void LateUpdate()
    {
        if (player == null || dungeonMap == null) return;

        Vector3 pos = player.transform.position;

        if (floor != null) floor.transform.position = new Vector3(pos.x, pos.y, floor.transform.position.z);
        if (sky != null) sky.transform.position = new Vector3(pos.x, pos.y, sky.transform.position.z);
        if (backGround != null) backGround.transform.position = new Vector3(pos.x, pos.y, backGround.transform.position.z);
        
        if (leftSide != null) leftSide.transform.position = new Vector3(pos.x, pos.y, leftSide.transform.position.z);
        if (rightSide != null) rightSide.transform.position = new Vector3(pos.x, pos.y, rightSide.transform.position.z);

        UpdateSideWallsVisibility();
    }

    void UpdateSideWallsVisibility()
    {
        Vector2Int forward = DirectionToVector(player.facing);
        Vector2Int leftVec = new Vector2Int(-forward.y, forward.x);
        Vector2Int rightVec = new Vector2Int(forward.y, -forward.x);
        Vector2Int currentPos = player.position;
        Vector2Int leftCell = currentPos + leftVec;
        Vector2Int rightCell = currentPos + rightVec;

        if (leftSide != null)
        {
            bool hayParedIzquierda = dungeonMap.BlocksView(leftCell.x, leftCell.y);
            leftSide.enabled = hayParedIzquierda;
        }

        if (rightSide != null)
        {
            bool hayParedDerecha = dungeonMap.BlocksView(rightCell.x, rightCell.y);
            rightSide.enabled = hayParedDerecha;
        }
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
}
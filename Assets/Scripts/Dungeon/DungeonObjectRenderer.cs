using UnityEngine;
using System.Collections.Generic;

public class DungeonObjectRenderer : MonoBehaviour
{
    public DungeonMap dungeonMap;
    public PlayerGridMovement player;

    public int viewDistance = 3;
    public int depthOffset = 100;

    [Header("Configuración de Tiles")]
    public List<TileVisualSet> tileVisuals;
    public TileVisualSet defaultVisuals;

    [System.Serializable]
    public class DepthLayer
    {
        public SpriteRenderer floor;
        public SpriteRenderer corridor;
        public SpriteRenderer frontWall;

        public SpriteRenderer leftFloor;
        public SpriteRenderer rightFloor;

        public SpriteRenderer leftWall;
        public SpriteRenderer rightWall;

        public SpriteRenderer frontLeftWall;
        public SpriteRenderer frontRightWall;
    }

    public DepthLayer[] layers;

    void Awake()
    {
        ApplySortingOrders();
    }

    void LateUpdate()
    {
        RenderDungeon();
    }

    Sprite GetSprite(Sprite[] collection, int depthIndex)
    {
        if (collection == null || collection.Length == 0) return null;
        if (depthIndex < collection.Length) return collection[depthIndex];
        return collection[collection.Length - 1];
    }

    TileVisualSet GetVisuals(TileType type)
    {
        foreach (var visual in tileVisuals)
            if (visual.type == type) return visual;

        return defaultVisuals != null ? defaultVisuals : tileVisuals[0];
    }

    void RenderDungeon()
    {
        Vector2Int forward = DirectionToVector(player.facing);
        Vector2Int left = new Vector2Int(-forward.y, forward.x);
        Vector2Int right = new Vector2Int(forward.y, -forward.x);
        Vector2Int origin = player.position;

        for (int i = 0; i < layers.Length; i++)
        {
            int d = i + 1;
            DepthLayer layer = layers[i];
            DisableLayer(layer);

            Vector2Int centerCell = origin + forward * d;
            Vector2Int prevCell = origin + forward * (d - 1);

            bool isFrontWall = dungeonMap.BlocksView(centerCell.x, centerCell.y);

            TileVisualSet floorVis = GetVisuals(dungeonMap.GetTile(prevCell.x, prevCell.y));
            TileVisualSet centerVis = GetVisuals(dungeonMap.GetTile(centerCell.x, centerCell.y));

            if (layer.floor)
            {
                layer.floor.sprite = GetSprite(floorVis.floor, i);
                layer.floor.enabled = true;
            }

            if (isFrontWall && layer.frontWall)
            {
                layer.frontWall.sprite = GetSprite(centerVis.frontWall, i);
                layer.frontWall.enabled = true;
            }
            else if (!isFrontWall && layer.corridor)
            {
                Sprite spriteToRender = GetSprite(centerVis.corridor, i);
                
                if (spriteToRender != null)
                {
                    layer.corridor.sprite = spriteToRender;
                    layer.corridor.enabled = true;
                }
            }

            Vector2Int leftTarget = prevCell + left;
            bool wallAtLeft = dungeonMap.BlocksView(leftTarget.x, leftTarget.y);
            TileVisualSet leftVis = GetVisuals(dungeonMap.GetTile(leftTarget.x, leftTarget.y));

            if (wallAtLeft && layer.leftWall)
            {
                layer.leftWall.sprite = GetSprite(leftVis.leftWall, i);
                layer.leftWall.enabled = true;
            }
            else if (!wallAtLeft && layer.leftFloor)
            {
                layer.leftFloor.sprite = GetSprite(leftVis.leftFloor, i);
                layer.leftFloor.enabled = true;
            }

            Vector2Int rightTarget = prevCell + right;
            bool wallAtRight = dungeonMap.BlocksView(rightTarget.x, rightTarget.y);
            TileVisualSet rightVis = GetVisuals(dungeonMap.GetTile(rightTarget.x, rightTarget.y));

            if (wallAtRight && layer.rightWall)
            {
                layer.rightWall.sprite = GetSprite(rightVis.rightWall, i);
                layer.rightWall.enabled = true;
            }
            else if (!wallAtRight && layer.rightFloor)
            {
                layer.rightFloor.sprite = GetSprite(rightVis.rightFloor, i);
                layer.rightFloor.enabled = true;
            }

            Vector2Int deepLeft = centerCell + left;
            Vector2Int deepRight = centerCell + right;

            if (!wallAtLeft && dungeonMap.BlocksView(deepLeft.x, deepLeft.y) && layer.frontLeftWall)
            {
                TileVisualSet vis = GetVisuals(dungeonMap.GetTile(deepLeft.x, deepLeft.y));
                layer.frontLeftWall.sprite = GetSprite(vis.frontLeftWall, i);
                layer.frontLeftWall.enabled = true;
            }

            if (!wallAtRight && dungeonMap.BlocksView(deepRight.x, deepRight.y) && layer.frontRightWall)
            {
                TileVisualSet vis = GetVisuals(dungeonMap.GetTile(deepRight.x, deepRight.y));
                layer.frontRightWall.sprite = GetSprite(vis.frontRightWall, i);
                layer.frontRightWall.enabled = true;
            }
        }
    }

    void ApplySortingOrders()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            int offset = ((layers.Length - 1 - i) * depthOffset) + 100;
            DepthLayer l = layers[i];

            SetOrder(l.floor, offset + 0);
            SetOrder(l.corridor, offset + 1);
            SetOrder(l.leftFloor, offset + 5);
            SetOrder(l.rightFloor, offset + 5);
            SetOrder(l.frontLeftWall, offset + 10);
            SetOrder(l.frontRightWall, offset + 10);
            SetOrder(l.leftWall, offset + 20);
            SetOrder(l.rightWall, offset + 20);
            SetOrder(l.frontWall, offset + 30);
        }
    }

    void SetOrder(SpriteRenderer sr, int order)
    {
        if (sr)
        {
            sr.sortingLayerName = "Dungeon";
            sr.sortingOrder = order;
        }
    }

    void DisableLayer(DepthLayer l)
    {
        if (l.floor) l.floor.enabled = false;
        if (l.corridor) l.corridor.enabled = false;
        if (l.frontWall) l.frontWall.enabled = false;
        if (l.leftFloor) l.leftFloor.enabled = false;
        if (l.rightFloor) l.rightFloor.enabled = false;
        if (l.leftWall) l.leftWall.enabled = false;
        if (l.rightWall) l.rightWall.enabled = false;
        if (l.frontLeftWall) l.frontLeftWall.enabled = false;
        if (l.frontRightWall) l.frontRightWall.enabled = false;
    }

    Vector2Int DirectionToVector(PlayerGridMovement.Direction dir)
    {
        switch (dir)
        {
            case PlayerGridMovement.Direction.North: return Vector2Int.up;
            case PlayerGridMovement.Direction.South: return Vector2Int.down;
            case PlayerGridMovement.Direction.East: return Vector2Int.right;
            case PlayerGridMovement.Direction.West: return Vector2Int.left;
        }
        return Vector2Int.zero;
    }
}
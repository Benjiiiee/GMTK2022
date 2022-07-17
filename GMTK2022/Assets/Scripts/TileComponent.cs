using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TileComponent : MonoBehaviour
{
    public TileType tileType;
    private Dictionary<TileType, Tile> tiles = new Dictionary<TileType, Tile>();

    public Tile tile { get => tiles[tileType]; }

    private void Awake() {
        tiles.Add(TileType.Empty, new EmptyTile(Vector3Int.zero));
        tiles.Add(TileType.Flat, new Tile(Vector3Int.zero, 1));
        tiles.Add(TileType.Wall, new WallTile(Vector3Int.zero));
        tiles.Add(TileType.Goal, new GoalTile(Vector3Int.zero));
        tiles.Add(TileType.CornerPosXPosZ, new CornerTile(Vector3Int.zero, 1, CornerTypes.PosXPosZ));
        tiles.Add(TileType.CornerPosXNegZ, new CornerTile(Vector3Int.zero, 1, CornerTypes.PosXNegZ));
        tiles.Add(TileType.CornerNegXPosZ, new CornerTile(Vector3Int.zero, 1, CornerTypes.NegXPosZ));
        tiles.Add(TileType.CornerNegXNegZ, new CornerTile(Vector3Int.zero, 1, CornerTypes.NegXNegZ));
        tiles.Add(TileType.SlopeDescentPosX, new SlopeTile(Vector3Int.zero, Vector3Int.right));
        tiles.Add(TileType.SlopeDescentNegX, new SlopeTile(Vector3Int.zero, Vector3Int.left));
        tiles.Add(TileType.SlopeDescentPosZ, new SlopeTile(Vector3Int.zero, Vector3Int.forward));
        tiles.Add(TileType.SlopeDescentNegZ, new SlopeTile(Vector3Int.zero, Vector3Int.back));
    }
}

public enum TileType
{
    Empty,
    Flat,
    Wall,
    Goal,
    CornerPosXPosZ,
    CornerPosXNegZ,
    CornerNegXPosZ,
    CornerNegXNegZ,
    SlopeDescentPosX,
    SlopeDescentNegX,
    SlopeDescentPosZ,
    SlopeDescentNegZ
}
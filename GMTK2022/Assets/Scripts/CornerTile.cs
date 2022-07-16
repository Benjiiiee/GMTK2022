using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerTile : Tile
{
    private CornerTypes type;

    public CornerTile(Vector3Int GridPosition, int MovementCost, CornerTypes type) : base(GridPosition, MovementCost) {
        this.type = type;
    }

    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        switch (type) {
            case CornerTypes.PosXPosZ:
                if(die.Direction.x < 0) {
                    die.Direction = new Vector3Int(0, 0, 1);
                }
                else if(die.Direction.z < 0) {
                    die.Direction = new Vector3Int(1, 0, 0);
                }
                break;
            case CornerTypes.PosXNegZ:
                if (die.Direction.x < 0) {
                    die.Direction = new Vector3Int(0, 0, -1);
                }
                else if (die.Direction.z > 0) {
                    die.Direction = new Vector3Int(1, 0, 0);
                }
                break;
            case CornerTypes.NegXPosZ:
                if (die.Direction.x > 0) {
                    die.Direction = new Vector3Int(0, 0, 1);
                }
                else if (die.Direction.z < 0) {
                    die.Direction = new Vector3Int(-1, 0, 0);
                }
                break;
            case CornerTypes.NegXNegZ:
                if (die.Direction.x > 0) {
                    die.Direction = new Vector3Int(0, 0, -1);
                }
                else if (die.Direction.z > 0) {
                    die.Direction = new Vector3Int(-1, 0, 0);
                }
                break;
            default:
                break;
        }
        return base.EnterTile(die, out nextDirection, out nextSteps, out nextGridPosition);
    }
}

public enum CornerTypes
{
    PosXPosZ,
    PosXNegZ,
    NegXPosZ,
    NegXNegZ
}
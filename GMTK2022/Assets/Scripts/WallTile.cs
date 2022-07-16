using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : Tile
{
    public WallTile(Vector3Int gridPosition) : base(gridPosition, 1) {
        this.GridPosition = gridPosition;
        this.MovementCost = 1;
    }
    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        if(die.Steps > MovementCost) {
            nextDirection = new Vector3Int(-die.Direction.x, 0, -die.Direction.z);
            nextSteps = die.Steps - MovementCost;
            nextGridPosition = die.GridPosition;
            return true;
        }
        nextSteps = die.Steps;
        nextGridPosition = die.GridPosition;
        nextDirection = die.Direction;
        return false;
    }

}

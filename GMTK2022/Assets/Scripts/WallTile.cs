using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallTile : Tile
{
    public WallTile(Vector3Int gridPosition) : base(gridPosition, 1) {
        this.GridPosition = gridPosition;
        this.MovementCost = 1;
    }
    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        if(die.Steps > MovementCost) {
            if (die.Direction.y != 0) {
                nextDirection = Vector3Int.down;
                nextSteps = Math.Max(die.Steps - MovementCost, 1);
            }
            else {
                nextDirection = new Vector3Int(-die.Direction.x, 0, -die.Direction.z);
                nextSteps = die.Steps - MovementCost;
            }
            nextGridPosition = die.GridPosition;
            return true;
        }
        nextSteps = die.Steps;
        nextGridPosition = die.GridPosition;
        nextDirection = die.Direction;
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmptyTile : Tile
{
    public EmptyTile(Vector3Int gridPosition) : base(gridPosition, 1) {
        this.GridPosition = gridPosition;
        this.MovementCost = 1;
    }

    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        if (die.Steps <= 0) {
            nextDirection = new Vector3Int(0, -1, 0);
            nextSteps = die.Steps;
        }
        else {
            nextDirection = new Vector3Int(die.Direction.x, -1, die.Direction.z);
            nextSteps = die.Steps - MovementCost;
        }
        nextGridPosition = GridPosition;
        return true;
    }

}

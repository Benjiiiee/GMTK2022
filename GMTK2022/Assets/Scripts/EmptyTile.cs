using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmptyTile : Tile
{
    public EmptyTile(Vector3Int gridPosition) : base(gridPosition, 0) {
        this.GridPosition = gridPosition;
        this.MovementCost = 0;
    }

    public override bool EnterTile(Vector3Int currentGridPosition, Vector3Int currentDirection, int currentSteps, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        nextDirection = Vector3Int.down;
        nextSteps = currentSteps;
        nextGridPosition = GridPosition;
        return true;
    }

}

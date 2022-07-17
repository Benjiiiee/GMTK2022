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
        // If entering an EmptyTile from the side:
        if (die.Direction.x != 0 || die.Direction.z != 0) {
            // Check if the die can pay the movement cost:
            if (die.Steps >= MovementCost) {
                // If it can, it pays the cost and moves into this tile
                nextSteps = die.Steps - MovementCost;
                nextGridPosition = GridPosition;
                // If it has no steps left, change its direction to down
                if(nextSteps == 0) {
                    nextDirection = Vector3Int.down;
                    nextSteps = Math.Max(die.Steps, MovementCost); // Falling is free!
                }
                else {
                    nextDirection = new Vector3Int(die.Direction.x, -1, die.Direction.z);
                }
                return true;
            }
            else {
                nextSteps = die.Steps;
                nextGridPosition = die.GridPosition;
                nextDirection = die.Direction;
                return false;
            }
        }
        // If entering an EmptyTile from the top:
        else {
            // Fall
            nextDirection = die.Direction;
            nextSteps = Math.Max(die.Steps, MovementCost); // Falling is free!
            nextGridPosition = GridPosition;
            return true;
        }
    }

}

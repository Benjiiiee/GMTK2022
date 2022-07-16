using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public int MovementCost;
    public Vector3Int GridPosition;

    public Tile(Vector3Int GridPosition, int MovementCost) {
        this.GridPosition = GridPosition;
        this.MovementCost = MovementCost;
    }

    // Default tile behavior: maintains direction, reduces steps by cost
    public virtual bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        if(die.Steps >= MovementCost) {
            nextDirection = die.LastHorizontalDirection;
            nextSteps = Math.Clamp(die.Steps - MovementCost, 0, int.MaxValue);
            nextGridPosition = GridPosition;
            return true;
        }

        nextDirection = die.Direction;
        nextSteps = die.Steps;
        nextGridPosition = die.GridPosition;
        return false;
    } 
}

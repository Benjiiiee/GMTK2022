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
    public virtual bool EnterTile(Vector3Int currentGridPosition, Vector3Int currentDirection, int currentSteps, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        if(currentSteps >= MovementCost) {
            nextDirection = new Vector3Int(currentDirection.x, 0, currentDirection.z);
            nextSteps = Math.Clamp(currentSteps - MovementCost, 0, int.MaxValue);
            nextGridPosition = GridPosition;
            return true;
        }

        nextDirection = currentDirection;
        nextSteps = currentSteps;
        nextGridPosition = currentGridPosition;
        return false;
    } 
}

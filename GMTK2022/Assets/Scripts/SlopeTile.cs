using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlopeTile : Tile
{
    public Vector3Int DescentDirection;

    public SlopeTile(Vector3Int GridPosition, Vector3Int DescentDirection) : base(GridPosition, 1) {
        this.DescentDirection = DescentDirection;
    }

    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {

        Vector3Int HorizontalDieDirection = new Vector3Int(die.Direction.x, 0, die.Direction.z);

        // If the die hits a rising slope, use rising movement cost:
        bool descending = HorizontalDieDirection != -DescentDirection;

        // It costs 1 to enter the slope.
        if(die.Steps >= MovementCost) {
            nextGridPosition = GridPosition;
            if (descending) {
                // If descending, gain the MovementCost instead of paying it
                nextSteps = die.Steps + MovementCost;
                // Direction is unchanged
                nextDirection = DescentDirection;
            }
            else {
                // If rising, pay the MovementCost twice
                nextSteps = Math.Max(die.Steps - 2 * MovementCost, 0);
                // If no more steps, direction is flipped and steps are increased (so the die can roll down the slope)
                if(nextSteps == 0) {
                    nextDirection = DescentDirection;
                    nextSteps += MovementCost;
                }
                // If steps remain, the die can climb up
                else {
                    nextDirection = -DescentDirection;
                    nextGridPosition = GridPosition + Vector3Int.up;
                }
            }
            return true;
        }
        nextDirection = die.Direction;
        nextSteps = die.Steps;
        nextGridPosition = die.GridPosition;
        return false;
    }
}

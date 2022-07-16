using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : Tile
{
    public GoalTile(Vector3Int GridPosition) : base(GridPosition, 1) {
    }

    public override bool EnterTile(DieController die, out Vector3Int nextDirection, out int nextSteps, out Vector3Int nextGridPosition) {
        return base.EnterTile(die, out nextDirection, out nextSteps, out nextGridPosition);
    }
}

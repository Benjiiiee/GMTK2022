using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour
{
    public DieTypes DieType;
    public int FaceValue;
    public int RemainingSteps;
    public Vector3Int Direction;
    public Vector3Int GridPosition;

    private IEnumerator MovementCoroutine;

    public void Shoot(Vector3Int direction) {
        Direction = direction;
        RemainingSteps = FaceValue;
        if (MovementCoroutine == null) {
            MovementCoroutine = Movement();
            StartCoroutine(MovementCoroutine);
        }
    }

    // Returns true if the die moved successfully; returns false if it stopped
    public bool NextStep() {
        // Look up the next tile in the current direction
        Tile nextTile = LevelManager.instance.GetTileInDirection(GridPosition, Direction);

        // Give the tile the die's state: Direction, RemainingSteps
        // The tile returns the die's next state: new Direction, new RemainingSteps, new GridPosition
        // If we moved, return true; otherwise, return false
        return nextTile.EnterTile(GridPosition, Direction, RemainingSteps, out Direction, out RemainingSteps, out GridPosition);
    }

    private IEnumerator Movement() {
        Vector3Int LastGridPosition = GridPosition;

        // Begin movement
        while (NextStep()) {
            float moveTimer = 0f;
            while(moveTimer < 1f) {
                transform.position = Vector3.Lerp(LastGridPosition, GridPosition, moveTimer);
                yield return null;
                moveTimer += Time.deltaTime;
            }
            transform.position = GridPosition;
            LastGridPosition = GridPosition;
        }

        // Movement finished

        MovementCoroutine = null;
    }
}

public enum DieTypes
{
    D4,
    D6,
    D8
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DieController : MonoBehaviour
{
    public static Action StepCompleted;
    public static Action MoveCompleted;

    public DieTypes DieType { get => CurrentDieType; set => ChangeDieType(value); }
    private DieTypes CurrentDieType;
    public int FaceValue;
    public int RemainingSteps;
    public Vector3Int Direction;
    public Vector3Int GridPosition;

    public GameObject D4Mesh;
    public GameObject D6Mesh;
    public GameObject D8Mesh;

    private IEnumerator MovementCoroutine;

    public void Spawn(Vector3Int StartPosition, DieTypes StartingType, int StartingFaceValue) {
        GridPosition = StartPosition;
        transform.position = StartPosition;
        DieType = StartingType;
        FaceValue = StartingFaceValue;
    }
    private void ChangeDieType(DieTypes newType) {
        switch (DieType) {
            case DieTypes.D4:
                D4Mesh.SetActive(false);
                break;
            case DieTypes.D6:
                D6Mesh.SetActive(false);
                break;
            case DieTypes.D8:
                D8Mesh.SetActive(false);
                break;
            default:
                break;
        }

        switch (newType) {
            case DieTypes.D4:
                D4Mesh.SetActive(true);
                break;
            case DieTypes.D6:
                D6Mesh.SetActive(true);
                break;
            case DieTypes.D8:
                D8Mesh.SetActive(true);
                break;
            default:
                break;
        }

        CurrentDieType = newType;
    }

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
            if (StepCompleted != null) StepCompleted();
        }

        // Movement finished
        if(MoveCompleted != null) MoveCompleted();
        MovementCoroutine = null;
    }
}

public enum DieTypes
{
    D4,
    D6,
    D8
}
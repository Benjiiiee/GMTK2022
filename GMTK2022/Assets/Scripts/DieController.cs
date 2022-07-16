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
    public int Steps;
    public Vector3Int Direction { get => CurrentDirection; set => SetDirection(value); }
    private Vector3Int CurrentDirection;
    public Vector3Int LastHorizontalDirection;
    public Vector3Int GridPosition;

    public GameObject D4Mesh;
    public GameObject D6Mesh;
    public GameObject D8Mesh;

    public DirectionArrow[] DirectionArrows;

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

    private void SetDirection(Vector3Int newDir) {
        CurrentDirection = newDir;
        if (newDir.x != 0 || newDir.z != 0) LastHorizontalDirection = new Vector3Int(newDir.x, 0, newDir.z);
    }

    public void Shoot(Vector3Int direction) {
        Direction = direction;
        Steps = FaceValue;
        if (MovementCoroutine == null) {
            MovementCoroutine = Movement();
            StartCoroutine(MovementCoroutine);
        }
    }

    private void HideDirectionArrows() {
        foreach(DirectionArrow arrow in DirectionArrows) {
            arrow.Hide();
        }
    }

    private void ShowDirectionArrows() {
        foreach (DirectionArrow arrow in DirectionArrows) {
            arrow.Show();
        }
    }

    // Returns true if the die moved successfully; returns false if it stopped
    public bool NextStep() {
        // Look up the next tile in the current direction
        Tile nextTile = LevelManager.instance.GetTileInDirection(GridPosition, Direction);

        // Give the tile the die's state: Direction, RemainingSteps
        // The tile returns the die's next state: new Direction, new RemainingSteps, new GridPosition
        // If we moved, return true; otherwise, return false
        bool HasMoved = nextTile.EnterTile(this, out Vector3Int nextDirection, out int remainingSteps, out Vector3Int nextGridPosition);

        Direction = nextDirection;
        Steps = remainingSteps;
        GridPosition = nextGridPosition;

        return HasMoved;
    }

    private IEnumerator Movement() {
        HideDirectionArrows();
        Vector3Int LastGridPosition = GridPosition;

        // Begin movement
        while (NextStep()) {
            float moveTimer = 0f;
            float moveDuration = 0.5f;
            while(moveTimer < moveDuration) {
                transform.position = Vector3.Lerp(LastGridPosition, GridPosition, moveTimer / moveDuration);
                yield return null;
                moveTimer += Time.deltaTime;
            }
            transform.position = GridPosition;
            LastGridPosition = GridPosition;
            if (StepCompleted != null) StepCompleted();
        }

        // Movement finished
        if(MoveCompleted != null) MoveCompleted();
        ShowDirectionArrows();
        MovementCoroutine = null;
    }
}

public enum DieTypes
{
    D4,
    D6,
    D8
}
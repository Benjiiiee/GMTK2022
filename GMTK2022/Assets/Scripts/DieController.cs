using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DieController : MonoBehaviour
{
    public static Action StepCompleted;
    public static Action MoveCompleted;
    public static Action<DieTypes, int> StoreDie;

    public DieTypes DieType { get => CurrentDieType; set => ChangeDieType(value, FaceValue); }
    private DieTypes CurrentDieType;
    public int FaceValue { get => CurrentFaceValue; set => SetFaceValue(value); }
    private int CurrentFaceValue;
    public int Steps;
    public Vector3Int Direction { get => CurrentDirection; set => SetDirection(value); }
    private Vector3Int CurrentDirection;
    public Vector3Int LastHorizontalDirection;
    public Vector3Int GridPosition;

    public GameObject D4Mesh;
    public GameObject D6Mesh;
    public GameObject D8Mesh;
    private TextMeshProUGUI TXT_FaceValue;
    private Animator animator;

    public DirectionArrow[] DirectionArrows;

    private IEnumerator MovementCoroutine;

    private void OnEnable()
    {
        StoredDiceController.RetrieveDie += RetrieveDie;
    }

    private void OnDisable()
    {
        StoredDiceController.RetrieveDie -= RetrieveDie;
    }

    public void Spawn(Vector3Int StartPosition, DieTypes StartingType, int StartingFaceValue)
    {
        GridPosition = StartPosition;
        transform.position = StartPosition;
        DieType = StartingType;
        FaceValue = StartingFaceValue;

        if (StoreDie != null)
        {
            if (StartingType != DieTypes.D4) StoreDie(DieTypes.D4, 4);
            if (StartingType != DieTypes.D6) StoreDie(DieTypes.D6, 6);
            if (StartingType != DieTypes.D8) StoreDie(DieTypes.D8, 8);
        }
    }
    private void ChangeDieType(DieTypes newType, int faceValue)
    {
        switch (DieType)
        {
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

        switch (newType)
        {
            case DieTypes.D4:
                D4Mesh.SetActive(true);
                animator = D4Mesh.GetComponentInChildren<Animator>();
                TXT_FaceValue = D4Mesh.GetComponentInChildren<TextMeshProUGUI>();
                break;
            case DieTypes.D6:
                D6Mesh.SetActive(true);
                animator = D6Mesh.GetComponentInChildren<Animator>();
                TXT_FaceValue = D6Mesh.GetComponentInChildren<TextMeshProUGUI>();
                break;
            case DieTypes.D8:
                D8Mesh.SetActive(true);
                animator = D8Mesh.GetComponentInChildren<Animator>();
                TXT_FaceValue = D8Mesh.GetComponentInChildren<TextMeshProUGUI>();
                break;
            default:
                break;
        }

        FaceValue = faceValue;
        CurrentDieType = newType;
    }

    private void RetrieveDie(DieTypes type, int faceValue)
    {
        if (StoreDie != null) StoreDie(DieType, FaceValue);
        ChangeDieType(type, faceValue);
    }

    private void SetDirection(Vector3Int newDir)
    {
        CurrentDirection = newDir;
        if (newDir.x != 0 || newDir.z != 0) LastHorizontalDirection = new Vector3Int(newDir.x, 0, newDir.z);
    }

    public void Shoot(Vector3Int direction)
    {
        Direction = direction;
        Steps = FaceValue;
        if (MovementCoroutine == null)
        {
            MovementCoroutine = Movement();
            StartCoroutine(MovementCoroutine);
        }
        if (animator != null)
        {
            animator.SetTrigger("isrun");
        }
    }

    private void HideDirectionArrows()
    {
        foreach (DirectionArrow arrow in DirectionArrows)
        {
            arrow.Hide();
        }
    }

    private void ShowDirectionArrows()
    {
        foreach (DirectionArrow arrow in DirectionArrows)
        {
            arrow.Show();
        }
    }

    // Returns true if the die moved successfully; returns false if it stopped
    public bool NextStep()
    {
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

    private IEnumerator Movement()
    {
        HideDirectionArrows();
        HideFaceValue();
        Vector3Int LastGridPosition = GridPosition;

        // Begin movement
        while (NextStep())
        {
            float moveTimer = 0f;
            float moveDuration = 0.5f;
            while (moveTimer < moveDuration)
            {
                transform.position = Vector3.Lerp(LastGridPosition, GridPosition, moveTimer / moveDuration);
                yield return null;
                moveTimer += Time.deltaTime;
            }
            transform.position = GridPosition;
            LastGridPosition = GridPosition;
            if (StepCompleted != null) StepCompleted();
        }

        // Movement finished
        if (MoveCompleted != null) MoveCompleted();
        ShowDirectionArrows();
        RollDie();
        ShowFaceValue();
        if (animator != null)
        {
            animator.SetTrigger("isidle");
        }
        MovementCoroutine = null;

    }

    private void RollDie()
    {
        FaceValue = UnityEngine.Random.Range(1, GetMaxFaceValue());
    }

    private int GetMaxFaceValue()
    {
        switch (DieType)
        {
            case DieTypes.D4:
                return 4;
            case DieTypes.D6:
                return 6;
            case DieTypes.D8:
                return 8;
            default:
                return 0;
        }
    }

    private void SetFaceValue(int value)
    {
        CurrentFaceValue = value;
        if (TXT_FaceValue != null) TXT_FaceValue.text = value.ToString();
    }

    private void HideFaceValue()
    {
        if (TXT_FaceValue != null) TXT_FaceValue.enabled = false;
    }

    private void ShowFaceValue()
    {
        if (TXT_FaceValue != null) TXT_FaceValue.enabled = true;
    }

    public void StopMovement()
    {
        if (MovementCoroutine != null) StopCoroutine(MovementCoroutine);
    }
}

public enum DieTypes
{
    D4,
    D6,
    D8
}
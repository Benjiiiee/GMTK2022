using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public static Action LevelCompleted;

//    public GridManager gridManager;
    public GameObject DiePrefab;

    private DieController dieController;

    [Header("Level Parameters")]
    public Vector3Int StartingGridPosition;
    public int StartingFaceValue;
    public DieTypes StartingDieType;

    public Vector3Int EndingGridPosition;

    private Vector3Int LastValidGridPosition;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        DieController.MoveStarted += OnMoveStarted;
        DieController.StepCompleted += OnStepCompleted;
        DieController.MoveCompleted += OnMoveCompleted;
        GameManager.LoadingComplete += OnLoadingCompleted;
    }

    private void OnDisable() {
        DieController.MoveStarted -= OnMoveStarted;
        DieController.StepCompleted -= OnStepCompleted;
        DieController.MoveCompleted -= OnMoveCompleted;
        GameManager.LoadingComplete -= OnLoadingCompleted;
    }

    private void OnLoadingCompleted() {
        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel() {
        yield return new WaitForSeconds(0.1f);
        GridManager.instance.InitiateGrid();
        dieController = FindObjectOfType<DieController>();
        if(dieController == null) {
            GameObject DieInstance = Instantiate(DiePrefab, StartingGridPosition, Quaternion.identity);
            dieController = DieInstance.GetComponent<DieController>();
        }
        dieController.Spawn(StartingGridPosition, StartingDieType, StartingFaceValue);
    }

    public Tile GetTileInDirection(Vector3Int gridPosition, Vector3Int dir) {
        Vector3Int tilePos = gridPosition + dir;

        // Check for walls
        RaycastHit hit;
        if(Physics.Raycast(gridPosition, dir, out hit, 1f, LayerMask.GetMask("Wall"))){
            // return a Wall tile
            return new WallTile(tilePos);
        }
        //Debug.DrawLine(gridPosition, tilePos, Color.green, 1.0f);

        // Get tile
        GridAnchor anchor = GridManager.instance.GetGridAnchor(tilePos);
        if (anchor != null && anchor.tile != null) return anchor.tile;
        
        else {
            // If no tile exists at the desired position, check if there is a descending slope to stick to
            anchor = GridManager.instance.GetGridAnchor(tilePos + Vector3Int.down);
            if(anchor != null && anchor.tile != null && anchor.tile is SlopeTile) {
                return anchor.tile;
            }
            return new EmptyTile(tilePos);
        }
    }
    private void OnMoveStarted() {
        LastValidGridPosition = dieController.GridPosition;
    }

    private void OnStepCompleted() {
        // If the die is on a goal tile, win:
        GridAnchor anchor = GridManager.instance.GetGridAnchor(dieController.GridPosition);
        if (anchor != null && anchor.tile != null && anchor.tile is GoalTile) {
            dieController.StopMovement();
            if (LevelCompleted != null) LevelCompleted();
        }

        // If the die falls in the water (grid level Y<0), bring it back to the last position:
        if(dieController.GridPosition.y < 0) {
            Respawn();
        }
    }

    private void OnMoveCompleted() {
    }

    private void Respawn() {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine() {
        dieController.StopMovement();
        yield return new WaitForSeconds(1f);
        dieController.Teleport(LastValidGridPosition);
        dieController.FinishMovement();
    }

    private void OnDestroy() {
        instance = null;
    }

    public DieController GetDieController() {
        return dieController;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GridManager gridManager;
    public GameObject DiePrefab;

    private DieController dieController;

    [Header("Level Parameters")]
    public Vector3Int StartingGridPosition;
    public int StartingFaceValue;
    public DieTypes StartingDieType;

    public Vector3Int EndingGridPosition;

    private bool isInputEnabled;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        DieController.StepCompleted += OnStepCompleted;
        DieController.MoveCompleted += OnMoveCompleted;
    }

    private void Start() {
        InitializeLevel();
    }

    private void InitializeLevel() {
        gridManager.InitiateGrid();
        dieController = FindObjectOfType<DieController>();
        if(dieController == null) {
            GameObject DieInstance = Instantiate(DiePrefab, StartingGridPosition, Quaternion.identity);
            dieController = DieInstance.GetComponent<DieController>();
        }
        dieController.Spawn(StartingGridPosition, StartingDieType, StartingFaceValue);
        isInputEnabled = true;
    }

    private void Update() {
        if(isInputEnabled && Input.GetKeyDown(KeyCode.RightArrow)) {
            dieController.Shoot(Vector3Int.right);
            isInputEnabled = false;
        }
    }

    public Tile GetTileInDirection(Vector3Int gridPosition, Vector3Int dir) {
        Vector3Int tilePos = gridPosition + dir;
        GridAnchor anchor = gridManager.GetGridAnchor(tilePos);
        if (anchor != null && anchor.tile != null) return anchor.tile;
        else return new EmptyTile(tilePos);
    }

    private void OnStepCompleted() {
        if (dieController.GridPosition == EndingGridPosition) Debug.Log("Win!");
    }

    private void OnMoveCompleted() {
        isInputEnabled = true;
    }
}
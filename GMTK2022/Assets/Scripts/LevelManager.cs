using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

//    public GridManager gridManager;
    public GameObject DiePrefab;

    private DieController dieController;

    [Header("Level Parameters")]
    public Vector3Int StartingGridPosition;
    public int StartingFaceValue;
    public DieTypes StartingDieType;

    public Vector3Int EndingGridPosition;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        DieController.StepCompleted += OnStepCompleted;
        DieController.MoveCompleted += OnMoveCompleted;
        GameManager.LoadingComplete += OnLoadingCompleted;
    }

    private void OnDisable() {
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
        Debug.DrawLine(gridPosition, tilePos, Color.green, 1.0f);

        // Get tile
        GridAnchor anchor = GridManager.instance.GetGridAnchor(tilePos);
        if (anchor != null && anchor.tile != null) return anchor.tile;
        else return new EmptyTile(tilePos);
    }

    private void OnStepCompleted() {
        if (GridManager.instance.GetGridAnchor(dieController.GridPosition).tile != null && GridManager.instance.GetGridAnchor(dieController.GridPosition).tile is GoalTile) {
            dieController.StopMovement();
            GameManager.instance.GoToMainMenu();
        }
    }

    private void OnMoveCompleted() {
    }

    private void OnDestroy() {
        instance = null;
    }
}

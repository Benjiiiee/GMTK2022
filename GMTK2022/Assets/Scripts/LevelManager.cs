using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GridManager gridManager;
    public GameObject DiePrefab;

    private DieController dieController;

    public Vector3 StartingGridPosition;
    public Vector3 EndingGridPosition;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start() {
        InitializeLevel();
    }

    private void InitializeLevel() {
        gridManager.InitiateGrid();
        GameObject DieInstance = Instantiate(DiePrefab, StartingGridPosition, Quaternion.identity);
        dieController = DieInstance.GetComponent<DieController>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            dieController.Shoot(Vector3Int.right);
        }
    }

    public Tile GetTileInDirection(Vector3Int gridPosition, Vector3Int dir) {
        //TODO: Get the actual tile
        Vector3Int tilePos = gridPosition + dir;
        return gridManager.gridArray[tilePos.x, tilePos.y, tilePos.z].tile;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridAnchor : MonoBehaviour
{
    public Vector3Int gridPosition;

    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GridManager.instance;
    }

    private void Update()
    {
        SnapToGrid();
        gridManager.gridArray[gridPosition.x, gridPosition.y, gridPosition.z] = this;
    }

    private void SnapToGrid()
    {
        float posX = transform.localPosition.x;
        float posY = transform.localPosition.y;
        float posZ = transform.localPosition.z;

        int indexGridX = Mathf.RoundToInt(posX);
        int indexGridY = Mathf.RoundToInt(posY);
        int indexGridZ = Mathf.RoundToInt(posZ);

        gridPosition = new Vector3Int(indexGridX, indexGridY, indexGridZ);

        transform.localPosition = gridPosition;
    }

    public Tile tile;

    // TODO: Replace with actual tile generation logic
    private void Start() {
        tile = new EmptyTile(gridPosition);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    public float distance = 1f;
    public Vector3Int gridSize;
    public GridAnchor gridAnchor;
    public GridAnchor[,,] gridArray;

    public static GridManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public GridAnchor GetGridAnchor(Vector3Int gridPos) {
        if(gridPos.x < gridSize.x && gridPos.y < gridSize.y && gridPos.z < gridSize.z && gridPos.x >= 0 && gridPos.y >= 0 && gridPos.z >= 0) {
            return gridArray[gridPos.x, gridPos.y, gridPos.z];
        }
        return null;
    }

    public void InitiateGrid()
    {
        // Clear current grid, if it exists
        ClearGrid();

        gridArray = new GridAnchor[gridSize.x, gridSize.y, gridSize.z];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int z = 0; z < gridSize.z; z++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (gridAnchor != null)
                    {
                        GameObject temp = Instantiate(gridAnchor.gameObject, new Vector3(x, y, z), Quaternion.identity);
                        gridArray[x, y, z] = temp.GetComponent<GridAnchor>();
                        temp.GetComponent<GridAnchor>().gridPosition = new Vector3Int(x, y, z);
                        
                    }
                }
            }
        }
    }

    public void ClearGrid() {
        if(gridArray != null) {
            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    for (int z = 0; z < gridArray.GetLength(2); z++) {
                        if (gridArray[x, y, z] != null) DestroyImmediate(gridArray[x, y, z].gameObject);
                    }
                }
            }

            gridArray = null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float distance = 1f;
    public Vector3Int gridSize;
    public GridAnchor gridAnchor;
    public GridAnchor[,,] gridArray;

    public void InitiateGrid()
    {
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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnchor : MonoBehaviour
{
    public Vector3Int gridPosition;
    public Tile tile;

    // TODO: Replace with actual tile generation logic
    private void Start() {
        tile = new Tile(gridPosition, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnchor : MonoBehaviour
{
    public Vector3Int gridPosition;
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

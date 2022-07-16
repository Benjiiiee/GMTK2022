using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileComponent : MonoBehaviour
{
    public Tile tile = new Tile(Vector3Int.zero, 1);

    private void Awake() {
        tile = new Tile(Vector3Int.zero, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridAnchor : MonoBehaviour
{
    public Vector3Int gridPosition;

    private GridManager gridManager;
    public bool isSnappable = false;
    public MeshRenderer cube;

    private void Awake()
    {
        gridManager = GridManager.instance;
        //Add DropArea tag if snappable
        gameObject.tag = isSnappable ? "DropArea" : "Untagged";
        cube = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        SnapToGrid();
        if(gridManager != null) gridManager.SetGridAnchor(gridPosition, this);

        // Check for tiles
        Collider[] colliders = Physics.OverlapSphere(gridPosition, 0.2f, LayerMask.GetMask("Grid"));
        if(colliders.Length < 1) {
            tile = null;
            if(isSnappable == true)
            {
                if(cube != null)
                {
                    cube.enabled = true;
                }
            }
        }
        else for (int i = 0; i < colliders.Length; i++) {
            TileComponent tc = colliders[i].GetComponent<TileComponent>();
            if (tc != null && tc.tile != null) {
                tc.tile.GridPosition = gridPosition;
                tc.transform.position = gridPosition;
                tile = tc.tile;
                if(isSnappable && cube != null)
                    {
                        cube.enabled = false;
                    }
            }
        }
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
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}

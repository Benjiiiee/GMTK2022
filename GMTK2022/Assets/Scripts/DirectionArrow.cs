using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Vector3Int Direction;

    private DieController dieController;
    private MeshRenderer mesh;
    private Collider col;

    // Start is called before the first frame update
    void Start()
    {
        dieController = GetComponentInParent<DieController>();
        col = GetComponent<Collider>();
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    public void Show() {
        mesh.enabled = true;
    }

    public void Hide() {
        mesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mesh.enabled) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit = CastRay();

                if (hit.collider != null && hit.collider.Equals(col)) {
                    dieController.Shoot(Direction);
                }

            }

        }
    }

    private RaycastHit CastRay() {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, (worldMousePosFar - worldMousePosNear).magnitude, LayerMask.GetMask("UI"));

        return hit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public Material HighlightMaterial;

    private float timeSinceHoveredOver;
    private float maxHighlightTime = 0.2f;
    private MeshRenderer[] meshes;
    private Material[] originalMaterials;
    private bool isHighlighted;

    private void Start() {
        meshes = GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[meshes.Length];
        for(int i = 0; i < originalMaterials.Length; i++) {
            originalMaterials[i] = meshes[i].material;
        }
    }

    public void OnHover() 
    {
        isHighlighted = true;
        timeSinceHoveredOver = 0;
    }

    public void OnHoverExit() {
        for (int i = 0; i < meshes.Length; i++) {
            meshes[i].material = originalMaterials[i];
        }
    }

    private void Update() {
        if(isHighlighted && timeSinceHoveredOver == 0f) {
            for(int i = 0; i < meshes.Length;i++) {
                meshes[i].material = HighlightMaterial;
            }
        }

        if (isHighlighted) {
            timeSinceHoveredOver += Time.deltaTime;
            if (timeSinceHoveredOver > maxHighlightTime) {
                OnHoverExit();
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
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
}

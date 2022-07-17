using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    private Outline outline;
    private float timeSinceHoveredOver;
    private float maxHighlightTime = 0.2f;
    private float targetOpacity;
    private float currentOpacity;

    private void Awake() {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = new Color(39f/255f, 245f/255f, 226f/255f, 0.0f);
        outline.OutlineWidth = 10f;
    }

    public void OnHover() 
    {
        targetOpacity = 0.75f;
        timeSinceHoveredOver = 0f;
    }

    public void OnHoverExit() {
        targetOpacity = 0.0f;
    }

    private void Update() {
        currentOpacity = Mathf.MoveTowards(currentOpacity, targetOpacity, Time.deltaTime * 10.0f);
        outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, currentOpacity);

        if (targetOpacity > 0f) {
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

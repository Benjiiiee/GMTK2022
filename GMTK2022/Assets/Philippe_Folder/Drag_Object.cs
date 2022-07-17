using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Object : MonoBehaviour
{
    private Vector3 mOffSet;
    private float mZCoord;
    public string destinationTag = "DropArea";

    public Vector3 lastLocation;
    public bool isDraggable = false;
    public bool isDiceMoving = false;

    private HighlightOnHover highlight;

    private void Start() {
        highlight = gameObject.GetComponent<HighlightOnHover>();
    }

    private void OnMouseOver() {
        if (highlight != null) {
            highlight.OnHover();
        }
    }

    private void OnMouseDown()
    {
        if (isDraggable && isDiceMoving == false)
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffSet = gameObject.transform.position - GetMouseWorldPos();
            transform.GetComponent<Collider>().enabled = false;
            Collider[] colliders = transform.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                if(col != null)
                {
                    col.enabled = false;
                }
            }
            Cursor.visible = false;
            lastLocation = transform.position;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = 1;
        Vector3 point1 = Camera.main.ScreenToWorldPoint(mousePoint);
        mousePoint.z = 2;
        Vector3 point2 = Camera.main.ScreenToWorldPoint(mousePoint);
        Vector3 line = point2 - point1;

        Vector3 intersection;
        if (LinePlaneIntersection(out intersection, point1, line, Vector3.up, new Vector3(0, 3, 0))){
            return intersection;
        }

        return Vector3.zero;

    }

    private void OnMouseDrag()
    {
        if (isDraggable && isDiceMoving == false)
        {
            transform.position = new Vector3(GetMouseWorldPos().x, 3, GetMouseWorldPos().z)  /*+ mOffSet*/;
            if (Input.GetMouseButtonDown(1))
            {
                transform.rotation = Quaternion.Euler(new Vector3(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + 90f,
                transform.rotation.eulerAngles.z));
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDraggable && isDiceMoving == false)
        {
            var rayOrigin = Camera.main.transform.position;
            var rayDirection = GetMouseWorldPos() - Camera.main.transform.position;
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, LayerMask.GetMask("DropArea")))
            {
                if (hitInfo.transform.tag == destinationTag)
                {
                    if (CheckIfFree(hitInfo))
                    {
                        transform.position = hitInfo.transform.position;
                        UpdateValidLocation();
                    }
                    else
                    {
                        transform.position = lastLocation;
                    }
                }
                else
                {
                    transform.position = lastLocation;
                }
            }
            else
            {
                transform.position = lastLocation;
            }

            transform.GetComponent<Collider>().enabled = true;
            Collider[] colliders = transform.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                if (col != null)
                {
                    col.enabled = true;
                }
            }
            Cursor.visible = true;
        }
    }

    public void UpdateValidLocation()
    {
        lastLocation = transform.position;
    }

    public bool CheckIfFree(RaycastHit hit)
    {
        GridAnchor anchor = hit.collider.gameObject.GetComponentInParent<GridAnchor>();
        if (anchor != null)
        {
            if (anchor.tile == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint) {
        float length;
        float dotNumerator;
        float dotDenominator;
        Vector3 vector;
        intersection = Vector3.zero;

        //calculate the distance between the linePoint and the line-plane intersection point
        dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
        dotDenominator = Vector3.Dot(lineVec, planeNormal);

        if (dotDenominator != 0.0f) {
            length = dotNumerator / dotDenominator;

            vector = lineVec.normalized * length;

            intersection = linePoint + vector;

            return true;
        }

        else
            return false;
    }

}

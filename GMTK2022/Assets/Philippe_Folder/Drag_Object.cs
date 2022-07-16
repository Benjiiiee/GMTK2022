using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Object : MonoBehaviour
{
    private Vector3 mOffSet;
    private float mZCoord;
    public string destinationTag = "DropArea";

    public Vector3 lastLocation;

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffSet = gameObject.transform.position - GetMouseWorldPos();
        transform.GetComponent<Collider>().enabled = false;
        Cursor.visible = false;
        lastLocation = transform.position;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() /*+ mOffSet*/;
        if (Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(new Vector3(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + 90f,
            transform.rotation.eulerAngles.z));
        }
    }

    private void OnMouseUp()
    {
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = GetMouseWorldPos() - Camera.main.transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
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
        Cursor.visible = true;
    }

    public void UpdateValidLocation()
    {
        lastLocation = transform.position;
    }

    public bool CheckIfFree(RaycastHit hit)
    {
        GridAnchor anchor = hit.collider.gameObject.GetComponent<GridAnchor>();
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

}

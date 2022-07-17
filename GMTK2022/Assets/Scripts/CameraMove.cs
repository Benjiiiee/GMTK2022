using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform cam;
    public float smoothTime = 1f;
    private Vector3 velocity = Vector3.zero;
    public Transform camTarget;
  

    public Vector3[] positions = new Vector3[]
    {
        new Vector3(-21.63f, 21.1f, -24.19f),
        new Vector3(-24.19f, 21.1f, 21.63f),
        new Vector3(21.63f, 21.1f, 24.19f),
        new Vector3(24.19f, 21.1f, -21.63f)
    };

    public Vector3[] rotations = new Vector3[]
    {
        new Vector3(30f, 45f, 0f),
        new Vector3(30f , 135f, 0f),
        new Vector3(30f, -135f, 0f),
        new Vector3(30f, -45f, 0f)
    };

    public int startingPosition = 0;
    public int currentPosition = 0;
    public int oldPosition = 0;
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        currentPosition = startingPosition;
    }

    private void Update()
    {
        cam.position = Vector3.SmoothDamp(cam.position, positions[currentPosition], ref velocity, smoothTime);
        //cam.rotation = Quaternion.Euler(Vector3.SmoothDamp(cam.eulerAngles, rotations[currentPosition], ref velocity, smoothTime));
        //cam.localEulerAngles = Vector3.SmoothDamp(cam.eulerAngles, rotations[currentPosition], ref velocity, smoothTime);
        //cam.LookAt(Vector3.zero);
        cam.LookAt(new Vector3 (4,0,4));
    }

    public void ChangePosition(int oldPos, int currentPos)
    {

    }

    public void IncrementCamPos()
    {
        oldPosition = currentPosition;
        if(oldPosition == 3)
        {
            currentPosition = 0;
        }
        else
        {
            currentPosition = oldPosition + 1;
        }

        ChangePosition(oldPosition, currentPosition);
    }

    public void DecrementCamPos()
    {
        oldPosition = currentPosition;
        if (oldPosition == 0)
        {
            currentPosition = 3;
        }
        else
        {
            currentPosition = oldPosition - 1;
        }

        ChangePosition(oldPosition, currentPosition);
    }

}

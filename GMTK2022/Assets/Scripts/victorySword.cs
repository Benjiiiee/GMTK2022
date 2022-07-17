using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victorySword : MonoBehaviour
{

    public int victoryPoints;
    public GameObject startPos;
    public GameObject stepPos;
    public GameObject endPos;
    public float desiredDuration;
    public float elapsedTime;
    public float percentageComplete;
    public bool changing = false;

    public GameObject sword;

    private void Start()
    {
        sword.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Update()
    {
        if (victoryPoints == 1 && changing == true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            sword.gameObject.transform.position = Vector3.Lerp(startPos.transform.position, stepPos.transform.position, percentageComplete);
            
            if (percentageComplete >= 1)
            {
                changing = false;
                elapsedTime = 0;
            }
        }

        if (victoryPoints == 2 && changing == true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            sword.gameObject.transform.position = Vector3.Lerp(stepPos.transform.position, endPos.transform.position, percentageComplete);

            if (percentageComplete >= 1)
            {
                changing = false;
                elapsedTime = 0;
                sword.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

}

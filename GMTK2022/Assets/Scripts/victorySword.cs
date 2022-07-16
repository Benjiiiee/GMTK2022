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

    public GameObject sword;

    private void Start()
    {
        sword.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void Update()
    {
        if (victoryPoints == 1)
        {
            while (percentageComplete < 0.95f)
            {
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / desiredDuration;
                sword.gameObject.transform.position = Vector3.Lerp(startPos.transform.position, stepPos.transform.position, percentageComplete);
            }
        }
    }
    public void Rise()
    {
        if (victoryPoints == 1)
        {

        }

    }

}

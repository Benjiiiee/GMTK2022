using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victorySword : MonoBehaviour
{

    public int victoryPoints;
    public GameObject startPos;
    public GameObject stepPos;
    public GameObject endPos;

    public GameObject sword;

    private void Start()
    {
        
    }
    public void Rise()
    {
        if (victoryPoints == 0)
        {
            sword.gameObject.transform.position = new Vector3(0, 0, 0);

        }

        if (victoryPoints == 1)
        {

        }
    }
}

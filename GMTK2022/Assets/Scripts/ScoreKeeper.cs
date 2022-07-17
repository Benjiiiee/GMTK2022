using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    // Start is called before the first frame update

     public int diceUsed;
     public int scoreCollectible;

    private void Start()
    {
        diceUsed = 0;
        scoreCollectible = 0;
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DiceUp();
        }
    }
    */

    public void DiceUp()
    {
        diceUsed = diceUsed + 1;
    }

    public void ScoreUp()
    {
        scoreCollectible = scoreCollectible + 10;
    }
}

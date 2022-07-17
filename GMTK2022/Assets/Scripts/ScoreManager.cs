using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI diceUsedTxt;
    public TextMeshProUGUI scoreTxt;

    public GameObject scoreKeeper;

    private void Start()
    {
        scoreKeeper = GameObject.Find("ScoreKeeper");
    }

    private void Update()
    {
        diceUsedTxt.text = "Dice rolls: " + scoreKeeper.GetComponent<ScoreKeeper>().diceUsed.ToString();
        scoreTxt.text = scoreKeeper.GetComponent<ScoreKeeper>().scoreCollectible.ToString() + " Points";
    }
}

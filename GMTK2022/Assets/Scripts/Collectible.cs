using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int collectDelay;
    public bool collectable = false;
    private Coroutine myRoutine;
    public GameObject scoreKeeper;

    private void Start()
    {
        scoreKeeper = GameObject.Find("ScoreKeeper");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dice" && collectable == true)
        {
            collectable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        myRoutine = StartCoroutine(Timer());
        if (other.gameObject.tag == "Dice" && collectable == true)
        {
            scoreKeeper.GetComponent<ScoreKeeper>().ScoreUp();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Dice")
        {
            StopCoroutine(myRoutine);
            collectable = false;
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(collectDelay);
        collectable = true;
    }

}

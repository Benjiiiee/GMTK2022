using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using System;

public class victorySword : MonoBehaviour
{
    public static Action AnimationComplete;

    public int victoryPoints;
    public GameObject startPos;
    public GameObject stepPos;
    public GameObject endPos;
    public float desiredDuration;
    public float elapsedTime;
    public float percentageComplete;
    public bool changing = false;
    public bool playSound;

    public GameObject sword;

    private Splash splash;

    //camera shake
    public ShakeData MyShake;

    //endgame
    public GameObject endScreen;


    private void OnEnable() {
        GameManager.LoadingComplete += OnLoadingComplete;
        LevelManager.LevelCompleted += GainVictoryPoint;
    }

    private void OnDisable() {
        GameManager.LoadingComplete -= OnLoadingComplete;
        LevelManager.LevelCompleted -= GainVictoryPoint;
    }

    private void Awake() {
        splash = GetComponentInChildren<Splash>();
        splash.enabled = false;
        endScreen = GameObject.Find("EndGame");
    }

    private void Start()
    {
        sword.GetComponent<Rigidbody>().isKinematic = true;
        playSound = true;
        endScreen.SetActive(false);
    }

    private void OnLoadingComplete() {
        if (GameManager.instance.state == GameStates.Level1) {
            victoryPoints = 1;
            sword.transform.position = stepPos.transform.position;
        }
    }

    private void GainVictoryPoint() {
        victoryPoints++;
        changing = true;
    }

    private void Update()
    {
        if (victoryPoints == 1 && changing == true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            sword.gameObject.transform.position = Vector3.Lerp(startPos.transform.position, stepPos.transform.position, percentageComplete);
            
            if (playSound == true)
            {
                AudioManager.instance.PlaySound(SoundName.SwordRising);
                CameraShakerHandler.Shake(MyShake);
                playSound = false;
            }
            if (percentageComplete > 1f)
            {
                changing = false;
                elapsedTime = 0;
                percentageComplete = 0;
                playSound = true;
                if (AnimationComplete != null) AnimationComplete();
            }
        }



        if (victoryPoints == 2 && changing == true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            sword.gameObject.transform.position = Vector3.Lerp(stepPos.transform.position, endPos.transform.position, percentageComplete);

            if (playSound == true)
            {
                AudioManager.instance.PlaySound(SoundName.SwordRising);
                CameraShakerHandler.Shake(MyShake);
                playSound = false;
                
            }

            if (percentageComplete >= 1)
            {
                changing = false;
                elapsedTime = 0;
                percentageComplete = 0;
                sword.GetComponent<Rigidbody>().isKinematic = false;
                playSound = true;
                StartCoroutine(EndingCoroutine());
            }
        }
    }

    private IEnumerator EndingCoroutine() {
        splash.enabled = true;
        yield return new WaitForSeconds(5.0f);
        //if(AnimationComplete != null) AnimationComplete();
        endScreen.SetActive(true);
    }

}

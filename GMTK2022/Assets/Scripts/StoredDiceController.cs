using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class StoredDiceController : MonoBehaviour
{
    public static Action<DieTypes, int> RetrieveDie;

    public int StoredD4Value = 4;
    public int StoredD6Value = 6;
    public int StoredD8Value = 8;

    public bool isD4Stored;
    public bool isD6Stored;
    public bool isD8Stored;

    public TextMeshProUGUI TXT_D4;
    public TextMeshProUGUI TXT_D6;
    public TextMeshProUGUI TXT_D8;

    // Start is called before the first frame update
    private void OnEnable() {
        DieController.StoreDie += StoreDie;
    }

    private void OnDisable() {
        DieController.StoreDie -= StoreDie;
    }

    private void StoreDie(DieTypes type, int faceValue) {
        switch (type) {
            case DieTypes.D4:
                StoredD4Value = faceValue;
                isD4Stored = true;
                break;
            case DieTypes.D6:
                StoredD6Value = faceValue;
                isD6Stored = true;
                break;
            case DieTypes.D8:
                StoredD8Value = faceValue;
                isD8Stored = true;
                break;
            default:
                break;
        }
        UpdateGUI();
    }

    public void OnDieClicked(DieTypes type) {
        switch (type) {
            case DieTypes.D4:
                if (isD4Stored) {
                    if (RetrieveDie != null) RetrieveDie(DieTypes.D4, StoredD4Value);
                    isD4Stored = false;
                }
                break;
            case DieTypes.D6:
                if (isD6Stored) {
                    if (RetrieveDie != null) RetrieveDie(DieTypes.D6, StoredD6Value);
                    isD6Stored = false;
                }
                break;
            case DieTypes.D8:
                if (isD8Stored) {
                    if (RetrieveDie != null) RetrieveDie(DieTypes.D8, StoredD8Value);
                    isD8Stored = false;
                }
                break;
            default:
                break;
        }
        UpdateGUI();
    }

    public void OnD4Clicked() {
        OnDieClicked(DieTypes.D4);
    }

    public void OnD6Clicked() {
        OnDieClicked(DieTypes.D6);
    }

    public void OnD8Clicked() {
        OnDieClicked(DieTypes.D8);
    }

    private void UpdateGUI() {
        TXT_D4.enabled = isD4Stored;
        TXT_D6.enabled = isD6Stored;
        TXT_D8.enabled = isD8Stored;

        TXT_D4.text = StoredD4Value.ToString();
        TXT_D6.text = StoredD6Value.ToString();
        TXT_D8.text = StoredD8Value.ToString();
    }

}

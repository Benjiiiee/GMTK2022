using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void OnPlayButtonPressed() {
        GameManager.instance.LaunchGame();
    }

}

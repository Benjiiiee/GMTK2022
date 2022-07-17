using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water" && GameManager.instance.state == GameStates.Level1)
        {
            AudioManager.instance.PlaySound(SoundName.SwordSplash);
        }
    }
}

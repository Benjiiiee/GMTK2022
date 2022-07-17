using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            AudioManager.instance.PlaySound(SoundName.SwordSplash);
        }
    }
}

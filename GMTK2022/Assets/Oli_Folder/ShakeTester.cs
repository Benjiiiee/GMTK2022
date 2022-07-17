using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
public class ShakeTester : MonoBehaviour
{
    public ShakeData MyShake;
    private void Start()
    {
        CameraShakerHandler.Shake(MyShake);
    }
}


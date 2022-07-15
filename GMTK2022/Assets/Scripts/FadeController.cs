using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public float FadeDuration;

    private Image Overlay;

    private IEnumerator CurrentFade;

    private void OnEnable() {
        GameManager.FadeIn += FadeIn;
        GameManager.FadeOut += FadeOut;
    }
    private void OnDisable() {
        GameManager.FadeIn -= FadeIn;
        GameManager.FadeOut -= FadeOut;
    }

    private void Start() {
        Overlay = GetComponent<Image>();
    }

    private void FadeIn() {
        if (CurrentFade != null) StopCoroutine(CurrentFade);
        CurrentFade = FadeCoroutine(true, Color.black, FadeDuration);
        StartCoroutine(CurrentFade);
    }
    private void FadeOut() {
        if (CurrentFade != null) StopCoroutine(CurrentFade);
        CurrentFade = FadeCoroutine(false, Color.black, FadeDuration);
        StartCoroutine(CurrentFade);
    }

    private IEnumerator FadeCoroutine(bool isFadeIn, Color fadeColor, float duration) { 

        float startOpacity = isFadeIn ? 1.0f : 0.0f;
        float endOpacity = isFadeIn ? 0.0f : 1.0f;

        Overlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, startOpacity);

        float timer = duration;
        while(timer >= 0.0f) {
            float currentOpacity = Mathf.Lerp(startOpacity, endOpacity, duration - timer);
            Overlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, currentOpacity);
            yield return null;
            timer -= Time.deltaTime;
        }

        Overlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, endOpacity);
    }
}

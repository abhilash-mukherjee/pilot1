using System;
using System.Collections;
using UnityEngine;

public class MaterialFader : MonoBehaviour
{
    public Material material;
    [SerializeField] private float fadeDuration = 1.0f; // Default fade duration

    private void OnEnable()
    {
        SessionManager.OnSessionEnded += Fade;
        SessionManager.OnNewSessionCreated += UnFade;
    }
    private void OnDisable()
    {
        SessionManager.OnSessionEnded -= Fade;
        SessionManager.OnNewSessionCreated -= UnFade;
    }

    private void UnFade(SessionData data)
    {
        Color endColor = material.color;
        endColor.a = 1f;
        material.color = endColor;
    }

    private void Fade(SessionData data)
    {
        FadeMaterial();
    }

    public void FadeMaterial()
    {
        StartCoroutine(FadeMaterialCoroutine());
    }

    // Coroutine to gradually fade the material's opacity
    private IEnumerator FadeMaterialCoroutine()
    {
        Color startColor = material.color;
        Color endColor = material.color;
        endColor.a = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            material.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.color = endColor;
    }
}

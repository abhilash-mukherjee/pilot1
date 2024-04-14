using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    private int _currentSkybox;
    [SerializeField] private List<Cubemap> skyboxCubeMaps;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private float transitionDuration = 1f;  // Default to 1 second

    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += ChangeSkybox;
    }

    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= ChangeSkybox;
    }

    private void ChangeSkybox(SessionData data)
    {
        _currentSkybox = data.sessionParams.environment;
        if (_currentSkybox >= skyboxCubeMaps.Count) return;
        StopAllCoroutines();
        StartCoroutine(ChangeSkyboxCoroutine());
    }

    private IEnumerator ChangeSkyboxCoroutine()
    {
        Color originalColor = skyboxMaterial.GetColor("_Tint");

        // Fade to black
        float elapsedTime = 0;
        while (elapsedTime < transitionDuration / 2)
        {
            float progress = elapsedTime / (transitionDuration / 2);
            skyboxMaterial.SetColor("_Tint", Color.Lerp(originalColor, Color.black, progress));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the new cubemap
        skyboxMaterial.SetTexture("_Tex", skyboxCubeMaps[_currentSkybox]);

        // Fade back to original tint
        elapsedTime = 0;
        while (elapsedTime < transitionDuration / 2)
        {
            float progress = elapsedTime / (transitionDuration / 2);
            skyboxMaterial.SetColor("_Tint", Color.Lerp(Color.black, originalColor, progress));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

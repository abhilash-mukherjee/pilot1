using System;
using System.Collections;
using UnityEngine;

public class EnvironmentAudioManager : MonoBehaviour
{
    public AudioSource ambientSource;
    public AudioSource gameplaySource;
    public GameConfig gameConfig;
    public BoolData isSessionPaused;

    private bool isTransitioning = false;

    private void Start()
    {
        StartAmbientMode(null);
    }

    private void OnEnable()
    {
        SessionManager.OnSessionPaused += StartAmbientMode;
        SessionManager.OnSessionResumed += StartGameplayMode;
        SessionManager.OnSessionEnded += StartAmbientMode;
        SessionManager.OnNewSessionCreated += StartGameplayModeAfterDelay;
    }

    private void OnDisable()
    {
        SessionManager.OnSessionPaused -= StartAmbientMode;
        SessionManager.OnSessionResumed -= StartGameplayMode;
        SessionManager.OnSessionEnded -= StartAmbientMode;
        SessionManager.OnNewSessionCreated -= StartGameplayModeAfterDelay;
    }

    private void StartGameplayModeAfterDelay(SessionData data)
    {
        gameplaySource.Stop();
        StartCoroutine(GameplayModeCoroutine(gameConfig.SessionStartDelay));
    }

    IEnumerator GameplayModeCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (isSessionPaused.value == false) StartGameplayMode(null);
    }
    private Coroutine transitionCoroutine;

    // Method to start ambient mode
    public void StartAmbientMode(SessionData data)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionAudio(ambientSource, gameplaySource));
    }

    // Method to start gameplay mode
    public void StartGameplayMode(SessionData data)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);
        Debug.Log("**Transition into gameplay audio started");
        transitionCoroutine = StartCoroutine(TransitionAudio(gameplaySource, ambientSource));
    }

    // Coroutine to transition between audio sources
    private IEnumerator TransitionAudio(AudioSource fadeInSource, AudioSource fadeOutSource)
    {
        // Fade out the current source
        yield return FadeOutAudio(fadeOutSource);

        // Pause the faded-out source
        fadeOutSource.Pause();

        // Fade in the new source
        fadeInSource.Play();
        yield return FadeInAudio(fadeInSource);

        // Reset transition coroutine reference
        transitionCoroutine = null;
    }

    // Coroutine to fade out an audio source
    private IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float fadeDuration = 3.0f; // Modify as needed

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    // Coroutine to fade in an audio source
    private IEnumerator FadeInAudio(AudioSource audioSource)
    {
        float endVolume = 1;
        float fadeDuration = 1.0f; // Modify as needed

        while (audioSource.volume < endVolume)
        {
            audioSource.volume += endVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}

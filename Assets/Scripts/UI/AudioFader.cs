using System;
using System.Collections;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]private float fadeDuration = 1.0f; // Default fade duration

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        CubeController.OnCubeCollidedObject += Collided;
        SessionManager.OnSessionEnded += SessionEnded;
    }
    
    private void OnDisable()
    {
        CubeController.OnCubeCollidedObject -= Collided;
        SessionManager.OnSessionEnded -= SessionEnded;
    }

    private void SessionEnded(SessionData data)
    {
        FadeAudio();
    }

    private void Collided(GameObject obj)
    {
        if (obj != gameObject) return;
        FadeAudio();
    }

    // Method to fade the audio volume to zero
    public void FadeAudio()
    {
        StartCoroutine(FadeAudioCoroutine());
    }

    // Coroutine to gradually fade the audio volume to zero
    private IEnumerator FadeAudioCoroutine()
    {
        float startVolume = _audioSource.volume;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = 0f;
    }

}

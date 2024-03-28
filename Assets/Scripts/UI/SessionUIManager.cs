using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionUIManager : MonoBehaviour
{

    [SerializeField]
    private SessionUISuperClass sessionStartUI, gameplayUI, endUI;
    [SerializeField] private Sprite pausedSprite, playingSprite;
    [SerializeField] private UnityEngine.UI.Image image;


    [SerializeField]
    private GameConfig gameConfig;

    private SessionData _currentSessionData;

    private void Start()
    {
        sessionStartUI.ShowUI(null);
    }
    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += NewSessionCreated;
        SessionManager.OnSessionEnded += SessionEnded;
        SessionManager.OnSessionPaused += PauseUI;
        SessionManager.OnSessionResumed += ResumeUI;
    }
    
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= NewSessionCreated;
        SessionManager.OnSessionEnded -= SessionEnded;
        SessionManager.OnSessionPaused -= PauseUI;
        SessionManager.OnSessionResumed -= ResumeUI;
    }

    private void PauseUI(SessionData data)
    {
        image.sprite = pausedSprite;
    }

    private void ResumeUI(SessionData data)
    {
        image.sprite = playingSprite;
    }
    private void NewSessionCreated(SessionData data)
    {
        _currentSessionData = data;
        sessionStartUI.ShowUI(data);
        endUI.HideUI(_currentSessionData);
        gameplayUI.HideUI(data);
        image.sprite = playingSprite;
        StopAllCoroutines();
        StartCoroutine(GameplayStarted());
    }

    IEnumerator GameplayStarted()
    {
        yield return new WaitForSeconds(gameConfig.SessionStartDelay);
        sessionStartUI.HideUI(_currentSessionData);
        endUI.HideUI(_currentSessionData);
        gameplayUI.ShowUI(_currentSessionData);
    }

    private void SessionEnded(SessionData data)
    {
        _currentSessionData = null;
        sessionStartUI.HideUI(null);
        gameplayUI.HideUI(_currentSessionData);
        endUI.ShowUI(_currentSessionData);
        StopAllCoroutines();
        StartCoroutine(ResetUI());
    }

    IEnumerator ResetUI()
    {
        yield return new WaitForSeconds(gameConfig.PostSessionEndResetTime);
        endUI.HideUI(_currentSessionData);
        sessionStartUI.ShowUI(null);
        gameplayUI.HideUI(_currentSessionData);
    }
}

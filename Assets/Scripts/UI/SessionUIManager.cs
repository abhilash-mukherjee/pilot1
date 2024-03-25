using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionUIManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI sessionStartTextUI;

    [SerializeField]
    private GameObject StartUIHolder, EndUIHolder, ScoreUIHolder, TimerUIHolder;

    [SerializeField]
    private GameConfig gameConfig;

    private SessionData _currentSessionData;

    private void Start()
    {
        sessionStartTextUI.text = "Waiting for new session to start";
        StartUIHolder.SetActive(true);
    }
    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += NewSessionCreated;
        SessionManager.OnSessionEnded += SessionEnded;
    }
    
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= NewSessionCreated;
        SessionManager.OnSessionEnded -= SessionEnded;
    }


    private void NewSessionCreated(SessionData data)
    {
        _currentSessionData = data;
        sessionStartTextUI.text = $"New session created. /n code: {_currentSessionData.code} /n module: {_currentSessionData.module}";
        StartUIHolder.SetActive(true);
        EndUIHolder.SetActive(false);
        ScoreUIHolder.SetActive(false);
        TimerUIHolder.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(GameplayStarted());
    }

    IEnumerator GameplayStarted()
    {
        yield return new WaitForSeconds(gameConfig.SessionStartDelay); 
        StartUIHolder.SetActive(false);
        EndUIHolder.SetActive(false);
        ScoreUIHolder.SetActive(true);
        TimerUIHolder.SetActive(true);
    }

    private void SessionEnded(SessionData data)
    {
        _currentSessionData = null;
        StartUIHolder.SetActive(false);
        ScoreUIHolder.SetActive(false);
        TimerUIHolder.SetActive(false);
        EndUIHolder.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(GameplayEnded());
    }

    IEnumerator GameplayEnded()
    {
        yield return new WaitForSeconds(gameConfig.PostSessionEndResetTime);
        EndUIHolder.SetActive(false);
        StartUIHolder.SetActive(true);
        ScoreUIHolder.SetActive(false);
        TimerUIHolder.SetActive(false);
        sessionStartTextUI.text = "Waiting for new session to start";
    }
}

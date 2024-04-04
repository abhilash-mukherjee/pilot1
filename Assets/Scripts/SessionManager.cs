using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Text;

public class SessionManager : MonoBehaviour
{
    public delegate void SessionDataHandler(SessionData data);
    public static event SessionDataHandler OnNewSessionCreated, OnSessionPaused, OnSessionResumed, OnSessionEnded;

    [SerializeField]
    private GameConfig gameConfig;

    [SerializeField]
    private BoolData isSessionPaused;

    private SessionData _sessionData;

    void Start()
    {
        _sessionData = new SessionData();
        ResetSessionData();
        StartCoroutine(RecursiveCoroutine());
    }

    private void OnEnable()
    {
        TimerManager.OnTimerEnd += TimerEnd;
    }

    private void OnDisable()
    {
        TimerManager.OnTimerEnd -= TimerEnd;
    }

    private void TimerEnd(SessionData data)
    {
        if (data.id != _sessionData.id) return;
        EndSession();
        PostRequest(gameConfig.HTTPEndSessionRequestURL);
    }

    IEnumerator RecursiveCoroutine()
    {
        yield return new WaitForSecondsRealtime(gameConfig.GetCurrentSessionFromServerInterval);
        GetRequest(gameConfig.HTTPGetCurrentSessionRequestURL);
        yield return StartCoroutine(RecursiveCoroutine());
    }

    void PostRequest(string uri)
    {
        var webRequest = new UnityWebRequest(uri, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Add the required header
        webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);

        webRequest.SendWebRequest();
    }

    void GetRequest(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);
        webRequest.SendWebRequest().completed += (op) => { ProcessData(webRequest); };

    }

    void ProcessData(UnityWebRequest webRequest)
    {
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:

            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + webRequest.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + webRequest.error);
                break;

            case UnityWebRequest.Result.Success:
                {
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(webRequest.downloadHandler.text);

                    //Ongoing Session Ended
                    if (responseData.sessionData == null || string.IsNullOrEmpty(responseData.sessionData.id))
                    {
                        if (!string.IsNullOrEmpty(_sessionData.id))
                        {
                            EndSession();
                        }
                        else
                        {
                            Debug.Log("No session anywhere");
                            return;
                        }
                    }

                    //New session detected
                    else if (!string.IsNullOrEmpty(responseData.sessionData.id) && string.IsNullOrEmpty(_sessionData.id)
                        && responseData.sessionData.status == "NOT_STARTED")
                    {
                        _sessionData = responseData.sessionData;
                        _sessionData.status = "RUNNING";
                        PostRequest(gameConfig.HTTPStartSessionRequestURL);
                        OnNewSessionCreated?.Invoke(_sessionData);
                        isSessionPaused.value = false;
                        Debug.Log($"New Session: {_sessionData.id}; CREATED");
                        if (_sessionData.status == "PAUSED")
                        {
                            PauseSession();
                        }
                    }


                    //Session paused or resumed
                    else if (responseData.sessionData.id == _sessionData.id)
                    {
                        if (responseData.sessionData.status == "PAUSED" && _sessionData.status == "RUNNING")
                        {
                            PauseSession();
                        }
                        else if (responseData.sessionData.status == "RUNNING" && _sessionData.status == "PAUSED")
                        {
                            ResumeSession();
                        }
                    }
                    else if (responseData.sessionData.id != _sessionData.id)
                    {
                        Debug.LogError("A different session request sent while this session is running.");
                    }

                    Debug.Log("Mega Log:\n" +
                                "Received: " + webRequest.downloadHandler.text + "\n" +
                                "Module: " + responseData.sessionData.module + "\n" +
                                "Code: " + responseData.sessionData.id + "\n"
                             );


                }
                break;
        }
    }

    private void EndSession()
    {
        OnSessionEnded?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.id}; ENDED");
        ResetSessionData();
    }

    private void ResumeSession()
    {
        _sessionData.status = "RUNNING";
        isSessionPaused.value = false;
        OnSessionResumed?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.id}; RESUMED");
    }

    private void PauseSession()
    {
        isSessionPaused.value = true;
        _sessionData.status = "PAUSED";
        OnSessionPaused?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.id}; PAUSED");
    }

    private void ResetSessionData()
    {
        _sessionData.id = "";
        _sessionData.sessionParams = null;
        _sessionData.status = "";
        _sessionData.module = "";
    }
}

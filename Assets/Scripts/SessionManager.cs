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
        if (data.code != _sessionData.code) return;
        EndSession();
        PostRequest(gameConfig.HTTPEndSessionRequestURL, "{ \"secret\": \"fyftyfytfy\"}");
    }

    IEnumerator RecursiveCoroutine()
    {
        yield return new WaitForSecondsRealtime(gameConfig.GetCurrentSessionFromServerInterval);
        GetRequest(gameConfig.HTTPGetCurrentSessionRequestURL);
        yield return StartCoroutine(RecursiveCoroutine());
    }

    void PostRequest(string uri, string body)
    {
        var webRequest = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        webRequest.SendWebRequest().completed += (op) => { ProcessData(webRequest); };
    }

    void GetRequest(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
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
                    if (responseData.sessionData == null || string.IsNullOrEmpty(responseData.sessionData.code))
                    {
                        if (!string.IsNullOrEmpty(_sessionData.code))
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
                    else if (!string.IsNullOrEmpty(responseData.sessionData.code) && string.IsNullOrEmpty(_sessionData.code))
                    {
                        _sessionData = responseData.sessionData;
                        OnNewSessionCreated?.Invoke(_sessionData);
                        isSessionPaused.value = false;
                        Debug.Log($"New Session: {_sessionData.code}; CREATED");
                        if (_sessionData.sessionStatus == "PAUSED")
                        {
                            PauseSession();
                        }
                    }


                    //Session paused or resumed
                    else if (responseData.sessionData.code == _sessionData.code)
                    {
                        if (responseData.sessionData.sessionStatus == "PAUSED" && _sessionData.sessionStatus == "RUNNING")
                        {
                            PauseSession();
                        }
                        else if (responseData.sessionData.sessionStatus == "RUNNING" && _sessionData.sessionStatus == "PAUSED")
                        {
                            ResumeSession();
                        }
                    }
                    else if (responseData.sessionData.code != _sessionData.code)
                    {
                        Debug.LogError("A different session request sent while this session is running.");
                    }

                    Debug.Log("Mega Log:\n" +
                                "Received: " + webRequest.downloadHandler.text + "\n" +
                                "Module: " + responseData.sessionData.module + "\n" +
                                "Code: " + responseData.sessionData.code + "\n"
                             );


                }
                break;
        }
    }

    private void EndSession()
    {
        OnSessionEnded?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.code}; ENDED");
        ResetSessionData();
    }

    private void ResumeSession()
    {
        _sessionData.sessionStatus = "RUNNING";
        isSessionPaused.value = false;
        OnSessionResumed?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.code}; RESUMED");
    }

    private void PauseSession()
    {
        isSessionPaused.value = true;
        _sessionData.sessionStatus = "PAUSED";
        OnSessionPaused?.Invoke(_sessionData);
        Debug.Log($"Session: {_sessionData.code}; PAUSED");
    }

    private void ResetSessionData()
    {
        _sessionData.code = "";
        _sessionData.sessionParams = null;
        _sessionData.sessionStatus = "";
        _sessionData.module = "";
    }
}

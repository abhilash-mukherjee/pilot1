using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class MetricsManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;
    [SerializeField] private SessionMetrics sessionMetrics;
    [SerializeField] private GameConfig gameConfig;
    private int _leftCubes, _rightCubes, _leftDodges, _rightDodges, _leftHits, _rightHits;

    private void OnEnable()
    {
        CubeController.OnCubeCollided += RecordMetrics;
        SessionManager.OnSessionEnded += SendMetrics;
        SessionManager.OnNewSessionCreated += ResetMetrics;
    }
    private void OnDisable()
    {
        CubeController.OnCubeCollided -= RecordMetrics;
        SessionManager.OnSessionEnded -= SendMetrics;
        SessionManager.OnNewSessionCreated -= ResetMetrics;
    }

    private void ResetMetrics(SessionData data)
    {
        _leftCubes = 0;
        _rightCubes = 0;
        _leftDodges = 0;
        _rightDodges = 0;
        _leftHits = 0;
        _rightHits = 0;
    }

    private void SendMetrics(SessionData data)
    {
        Debug.Log($"Session Ended. \n Report: \ntotal cubes: {_leftCubes + _rightCubes}, Left cubes: {_leftCubes},  Right cubes: {_rightCubes} " +
            $"\ntotal Hits: {_leftHits + _rightHits}, Left Hits: {_leftHits},  Right Hits: {_rightHits}" +
            $"\ntotal Dodges: {_leftDodges + _rightDodges}, Left Dodges: {_leftDodges},  Right Dodges: {_rightDodges} " +
            $"\nScore = {score.value}");
        sessionMetrics.leftCubes = _leftCubes;
        sessionMetrics.rightCubes = _rightCubes;
        sessionMetrics.leftDodges = _leftDodges;
        sessionMetrics.rightDodges = _rightDodges;
        sessionMetrics.leftHits = _leftHits;
        sessionMetrics.rightHits = _rightHits;
        StartCoroutine(MetricsCoroutine(data.id));
    }

    IEnumerator MetricsCoroutine(string sessionId)
    {
        string uri = gameConfig.HTTPSendMetricsRequestURL;
        Debug.Log("#################Inside metrics coroutine. id =" + sessionId);
        string jsonData = "{\"id\":" + $" \"{sessionId}\"" + "," +  " \"sessionMetrics\": {"
                   + "\"score\": " + score.value + ","
                   + "\"rightCubes\": " + _rightCubes + ","
                   + "\"leftCubes\": " + _leftCubes + ","
                   + "\"rightHits\": " + _rightHits + ","
                   + "\"leftHits\": " + _leftHits + ","
                   + "\"leftDodges\": " + _leftDodges + ","
                   + "\"rightDodges\": " + _rightDodges
                   + "}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("unity-client-secret", gameConfig.Secret);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
        }
    }

    private void RecordMetrics(TargetSide side, EventType eventType)
    {
        if(side == TargetSide.LEFT)
        {
            _leftCubes++;
            if (eventType == EventType.HIT) _leftHits++;
            else _leftDodges++;
        }
        else
        {
            _rightCubes++;
            if (eventType == EventType.HIT) _rightHits++;
            else _rightDodges++;
        }
    }
}
public enum EventType
{
    HIT,DODGED
}
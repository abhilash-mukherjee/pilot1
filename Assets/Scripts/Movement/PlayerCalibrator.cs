using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Unity.XR.CoreUtils;

public class PlayerCalibrator : MonoBehaviour
{
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private Vector3Data playerMeanPosition;
    private List<Vector3> _headPositions;

    private void Awake()
    {
        _headPositions = new List<Vector3>();
    }
    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += Calibrate;
    }

    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= Calibrate;
    }

    private void Calibrate(SessionData data)
    {
        _headPositions.Clear();
        Debug.Log("Calibration started");
        StartCoroutine(CollectHeadPositionCoroutine(gameConfig.CalibrationTime));

    }

    IEnumerator CollectHeadPositionCoroutine(int calibrationSecs)
    {
        int time = 0;
        while (time < calibrationSecs)
        {
            yield return new WaitForSecondsRealtime(1);
            _headPositions.Add(Camera.main.transform.position);
            Debug.Log("Positoon colledcted");
            time++;
        }
        Vector3 playerMeanPosition = Vector3.zero;
        _headPositions.ForEach(p => playerMeanPosition += p);
        playerMeanPosition /= (float)_headPositions.Count;
        this.playerMeanPosition.value = playerMeanPosition;
        var obj = new GameObject("Mean position");
        Debug.Log("Calibration ended");
        obj.transform.position = playerMeanPosition;
        _headPositions.Clear();
    }
}

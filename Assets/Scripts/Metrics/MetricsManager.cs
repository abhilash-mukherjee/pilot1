using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricsManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;

    private int _leftCubes, _rightCubes, _leftDodges, _rightDodges, _leftHits, _rightHits;

    private void OnEnable()
    {
        CubeController.OnCubeCollided += RecordMetrics;
        SessionManager.OnSessionEnded += SendMetrics;
    }
    private void OnDisable()
    {
        CubeController.OnCubeCollided -= RecordMetrics;
        SessionManager.OnSessionEnded -= SendMetrics;
    }

    private void SendMetrics(SessionData data)
    {
        Debug.Log($"Session Ended. \n Report: \ntotal cubes: {_leftCubes + _rightCubes}, Left cubes: {_leftCubes},  Right cubes: {_rightCubes} " +
            $"\ntotal Hits: {_leftHits + _rightHits}, Left Hits: {_leftHits},  Right Hits: {_rightHits}" +
            $"\ntotal Dodges: {_leftDodges + _rightDodges}, Left Dodges: {_leftDodges},  Right Dodges: {_rightDodges} " +
            $"\nScore = {score.value}");
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
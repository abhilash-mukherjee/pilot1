using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionParams
{
    public int duration;
    public int cubeGap;
    public int speed;
    public string targetSide;
    public int rightOffsetCentimeters;
    public int leftOffsetCentimeters;
    public bool isStanding;
    public int cubeScaleDecimeters;
    public int spawningDistanceMetres;
    public int spawnHeightDecimetres;
    public int zThresholdInMetres;
}

[System.Serializable]
public class SessionData
{
    public string module;
    public string id;
    public SessionParams sessionParams;
    public string status;
}

[System.Serializable]
public class ResponseData
{
    public SessionData sessionData;
}

public enum SessionStatus
{
    NO_ACTIVE_SESSION,
    SESSION_RUNNING,
    SESSION_PAUSED
}

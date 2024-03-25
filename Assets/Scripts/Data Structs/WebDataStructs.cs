using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionParams
{
    public int var1;
}

[System.Serializable]
public class SessionData
{
    public string module;
    public int duration;
    public string code;
    public SessionParams sessionParams;
    public string sessionStatus;
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

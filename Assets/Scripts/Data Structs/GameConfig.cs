using UnityEngine;

[CreateAssetMenu(fileName ="New Game Config", menuName ="Data Containers / Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _sessionStartDelay;
    [SerializeField] private int _calibrationTime;
    [SerializeField] private int _postSessionEndResetTime;
    [SerializeField] private float _getCurrentSessionFromServerInterval;
    [SerializeField] private string _hTTPGetCurrentSessionRequestURL;
    [SerializeField] private string _hTTPEndSessionRequestURL;
    [SerializeField] private string _hTTPStartSessionRequestURL;
    [SerializeField] private string _hTTPSendMetricsRequestURL;
    [SerializeField] private int _scoreIncrementOnDodge;
    [SerializeField] private string _secret;

    public int SessionStartDelay { get => _sessionStartDelay; }
    public int CalibrationTime{ get => _calibrationTime; }
    public int PostSessionEndResetTime { get => _postSessionEndResetTime; }
    public float GetCurrentSessionFromServerInterval { get => _getCurrentSessionFromServerInterval; }
    public string HTTPGetCurrentSessionRequestURL { get => _hTTPGetCurrentSessionRequestURL; }
    public string HTTPEndSessionRequestURL { get => _hTTPEndSessionRequestURL; }
    public string HTTPStartSessionRequestURL { get => _hTTPStartSessionRequestURL; }
    public string HTTPSendMetricsRequestURL { get => _hTTPSendMetricsRequestURL; }
    public int ScoreIncrementOnDodge { get => _scoreIncrementOnDodge;  }
    public string Secret { get => _secret; }
}

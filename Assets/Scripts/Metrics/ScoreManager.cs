using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private IntegerData score;
    [SerializeField] private GameConfig gameConfig;

    private void OnEnable()
    {
        CubeController.OnCubeCollided += UpdateScore;
        SessionManager.OnNewSessionCreated += ResetScore;
    }
    
    private void OnDisable()
    {
        CubeController.OnCubeCollided -= UpdateScore;
        SessionManager.OnNewSessionCreated -= ResetScore;
    }

    private void ResetScore(SessionData data)
    {
        score.value = 0;
    }

    private void UpdateScore(TargetSide side, EventType eventType)
    {
        if(eventType == EventType.DODGED)
        {
            score.value += gameConfig.ScoreIncrementOnDodge;
        }
    }
}

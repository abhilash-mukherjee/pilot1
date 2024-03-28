using UnityEngine;

public class GameplayUI : SessionUISuperClass
{
    [SerializeField] private GameObject uiReference;
    [SerializeField] private UnityEngine.UI.Image timerImage;
    [SerializeField] private AudioSource source;

    private int totalDuration; 

    private void OnEnable()
    {
        TimerManager.OnTimerTicked += Tick;
    }
    
    private void OnDisable()
    {
        TimerManager.OnTimerTicked -= Tick;
    }

    private void Tick(SessionParams sessionParams, int timeLeft)
    {
        timerImage.fillAmount = (float)(totalDuration - timeLeft) / (float)totalDuration;
    }

    public override void HideUI(SessionData data)
    {
        uiReference.SetActive(false);
        totalDuration = 0;
        source.Pause();
    }

    public override void ShowUI(SessionData data)
    {
        uiReference.SetActive(true);
        timerImage.fillAmount = 0f;
        if (data != null && data.sessionParams != null)
        {
            totalDuration = data.sessionParams.duration;
            source.Play();
        }
    }
}

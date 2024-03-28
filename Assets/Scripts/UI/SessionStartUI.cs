using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SessionStartUI : SessionUISuperClass
{
    [SerializeField] private TMPro.TextMeshProUGUI textMessage;
    [SerializeField] private string waitingForSessionMessage;
    [SerializeField] private string foundNewSessionMessage;
    [SerializeField] private GameObject uiObjectReference;
    [SerializeField] private Image timerImage;
    [SerializeField] private int timerAnimationTime;
    [SerializeField] private AudioSource sessionStartAudio;

    private int _totalTime;

    public override void ShowUI(SessionData data)
    {
        uiObjectReference.SetActive(true);
        timerImage.fillAmount = 0f;
        if(data != null)
        {
            textMessage.text = foundNewSessionMessage;
            AnimateTimer(timerAnimationTime);
            sessionStartAudio.Play();
        }
        else
        {
            textMessage.text = waitingForSessionMessage;
        }
    }

    public override void HideUI(SessionData data)
    {
        uiObjectReference.SetActive(false);
        timerImage.fillAmount = 1f;
    }

    public void AnimateTimer(int seconds)
    {
        _totalTime = seconds;
        StartCoroutine(FillTimer());
    }

    // Coroutine to gradually fill the timer image
    private IEnumerator FillTimer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _totalTime)
        {
            float fillAmount = elapsedTime / _totalTime;
            timerImage.fillAmount = fillAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fill amount is exactly 1 at the end
        timerImage.fillAmount = 1f;
    }

}
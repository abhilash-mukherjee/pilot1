using System.Collections;
using UnityEngine;

public class EndSessionUI : SessionUISuperClass
{
    [SerializeField] private TMPro.TextMeshProUGUI leftCubes, rightCubes, leftDodges, rightDodges, leftHits, rightHits, scoreUI;
    [SerializeField] private SessionMetrics metrics;
    [SerializeField] private IntegerData score;
    [SerializeField] private GameObject uiObjectReference;
    [SerializeField] private AudioSource source;

    public override void HideUI(SessionData data)
    {
        uiObjectReference.SetActive(false);
        source.Pause();
        SetUIState(false);
    }

    public override void ShowUI(SessionData data)
    {
        source.Play();
        uiObjectReference.SetActive(true);
        StartCoroutine(DisplayMetrics(0.1f));
    }

    IEnumerator DisplayMetrics(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SetUIState(true);
        leftCubes.text = metrics.leftCubes.ToString();
        rightCubes.text = metrics.rightCubes.ToString();
        leftDodges.text = metrics.leftDodges.ToString();
        rightDodges.text = metrics.rightDodges.ToString();
        leftHits.text = metrics.leftHits.ToString();
        rightHits.text = metrics.rightHits.ToString();
        scoreUI.text = score.value.ToString();

    }

    private void SetUIState(bool isActive)
    {
        leftCubes.gameObject.SetActive(isActive);
        rightCubes.gameObject.SetActive(isActive); 
        leftDodges.gameObject.SetActive(isActive); 
        rightDodges.gameObject.SetActive(isActive); 
        leftHits.gameObject.SetActive(isActive); 
        rightHits.gameObject.SetActive(isActive); 
        scoreUI.gameObject.SetActive(isActive); 
    }
}
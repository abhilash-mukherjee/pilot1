using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class SessionManager : MonoBehaviour
{
    public delegate void SessionDataHandler(SessionData data);
    public static event SessionDataHandler onSessionStarted, onSessionPaused, onSessionResumed, onSessionEnded;
    [SerializeField]
    private string getCurrentSessionUrl;
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    [SerializeField]
    [Range(0, 1)]
    private float timeScale = 1;
    void Start()
    {
        StartCoroutine(RecursiveCoroutine());
    }

    IEnumerator RecursiveCoroutine()
    {
        yield return new WaitForSecondsRealtime(6.0f);
        GetRequest(getCurrentSessionUrl);
        yield return StartCoroutine(RecursiveCoroutine());
    }

    void GetRequest(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.SendWebRequest().completed += (op) => { ProcessData(webRequest); };
            
    }

    void ProcessData(UnityWebRequest webRequest)
    {
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                {
                    Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                    text.text = webRequest.downloadHandler.text;
                }
                break;
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }
}

public struct SessionData
{
    public string sessionCode;
    public string moduleName;
    public int duration;
    public SessionStatus status;
}

public enum SessionStatus
{
    RUNNING,
    PAUSED,
    ENDED
}
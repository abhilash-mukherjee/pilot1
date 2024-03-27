using UnityEngine;
using Unity.XR.CoreUtils;

public class PlayerOrientationManager : MonoBehaviour
{

    private void OnEnable()
    {
        SessionManager.OnNewSessionCreated += Recentre;
    }
    private void OnDisable()
    {
        SessionManager.OnNewSessionCreated -= Recentre;
    }

    private void Recentre(SessionData data)
    {
        var xrOrigin = GetComponent<XROrigin>();
        var targetPos = Vector3.zero;
        targetPos.y = 1.36144f;
        xrOrigin.MoveCameraToWorldLocation(targetPos);
        xrOrigin.MatchOriginUpCameraForward(Vector3.up, Vector3.forward);
    }
}
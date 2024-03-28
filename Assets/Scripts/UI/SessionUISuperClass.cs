using System.Collections;
using UnityEngine;

public abstract class SessionUISuperClass : MonoBehaviour
{
    public abstract void ShowUI(SessionData data);
    public abstract void HideUI(SessionData data);
}

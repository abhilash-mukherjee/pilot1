using UnityEngine;

[CreateAssetMenu(fileName =("New Session Metrics"), menuName =("Shared Data / Session Metrics"))]
public class SessionMetrics : ScriptableObject
{

    public int leftCubes, rightCubes, leftDodges, rightDodges, leftHits, rightHits;

}



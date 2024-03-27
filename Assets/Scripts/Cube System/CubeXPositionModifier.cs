using UnityEngine;

[RequireComponent(typeof(CubeController))]
public class CubeXPositionModifier : MonoBehaviour
{
    [SerializeField] Vector3Data zThresholdForStoppingModification;
    private bool _isStanding;
    private Vector3 _positionVector = Vector3.zero;
    private void Awake()
    {
        _isStanding = GetComponent<CubeController>().IsStanding;
    }
    private void LateUpdate()
    {
        if (!_isStanding) return;
        if (transform.position.z < zThresholdForStoppingModification.value.z) return;
        _positionVector.x = Camera.main.transform.position.x;
        _positionVector.y = transform.position.y;
        _positionVector.z = transform.position.z;
        transform.position = _positionVector;
    }
}
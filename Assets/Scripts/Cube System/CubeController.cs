using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public delegate void CubeCollisionEventHandler(TargetSide side, EventType eventType);
    public static event CubeCollisionEventHandler OnCubeCollided;
    [SerializeField] private GameObject cubeModel;
    [SerializeField] private Vector3Data meanPosition;
    private TargetSide _targetSide;
    private bool _isStanding;
    private float _currrentScaleFactor;
    private int _speed;
    private float _zThresholdForStoppingModification;

    public bool IsStanding { get => _isStanding;  }

    public void InitiateCube(SessionParams sessionParams, TargetSide targetSide)
    {
        StartCoroutine(DestroyCube());
        _targetSide = targetSide;
        _isStanding = sessionParams.isStanding;
        _speed = sessionParams.speed;
        _zThresholdForStoppingModification = sessionParams.zThresholdInMetres;
        _currrentScaleFactor = RescaleCube(sessionParams.cubeScaleDecimeters);
        RepositionCube( (float) sessionParams.spawnHeightDecimetres / 10, sessionParams.spawningDistanceMetres, targetSide == TargetSide.LEFT ? sessionParams.rightOffsetCentimeters : -sessionParams.leftOffsetCentimeters);
        Debug.Log("Velocity: " + new Vector3(0, 0, -sessionParams.speed));
    }

    private void RepositionCube(float originY, float originZ, int XOffsetInCentimeters)
    {
        var startingX = _isStanding ? Camera.main.transform.position.x : meanPosition.value.x;
        transform.position = new Vector3( startingX + (float)XOffsetInCentimeters / 100, 0, 0)
            + new Vector3(0, originY, originZ);
        var offsetDirection = XOffsetInCentimeters > 0 ? 1 : -1;
        cubeModel.transform.localPosition += new Vector3(_currrentScaleFactor / 2, 0, 0) * offsetDirection;
    }

    private float RescaleCube(int cubeScaleDecimeters)
    {
        float scaleFactor = cubeScaleDecimeters < 1 ? 1 : cubeScaleDecimeters;
        scaleFactor /= 10;
        cubeModel.transform.localScale = Vector3.one * scaleFactor;
        return scaleFactor;
    }

    IEnumerator DestroyCube()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position = GetUpdatedPosition();
    }

    private Vector3 GetUpdatedPosition()
    {
        var delta = _speed* Time.deltaTime * -transform.forward;
        var newPos = transform.position + delta;
        if (_isStanding && transform.position.z > _zThresholdForStoppingModification) newPos.x = Camera.main.transform.position.x;
        return newPos;

    }
    public void OnCubeHitPlayer()
    {
        Debug.Log("Hit");
        OnCubeCollided?.Invoke(_targetSide, EventType.HIT);
    }

    public void OnCubeHitWall()
    {

        Debug.Log("Dodged");
        OnCubeCollided?.Invoke(_targetSide, EventType.DODGED);
    }

}
public enum TargetSide
{
    LEFT, RIGHT
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public delegate void CubeCollisionEventHandler(TargetSide side, EventType eventType);
    public static event CubeCollisionEventHandler OnCubeCollided;
    public delegate void CubeCollisionHandler(GameObject obj);
    public static event CubeCollisionHandler OnCubeCollidedObject;
    [SerializeField] private GameObject cubeModel;
    [SerializeField] private Vector3Data meanPosition;
    [SerializeField] private BoolData isSessionPaused;
    private TargetSide _targetSide;
    private bool _isStanding;
    private float _currrentScaleFactor;
    private float _xOffset;
    private int _speed;
    private float _zThresholdForStoppingModification;
    private bool _hasHitPlayer;
    public bool IsStanding { get => _isStanding;  }

    public void InitiateCube(SessionParams sessionParams, TargetSide targetSide)
    {
        _targetSide = targetSide;
        _isStanding = sessionParams.isStanding;
        _speed = sessionParams.speed;
        _zThresholdForStoppingModification = sessionParams.zThresholdInMetres;
        _currrentScaleFactor = RescaleCube(sessionParams.cubeScaleDecimeters);
        _xOffset = (float) (targetSide == TargetSide.LEFT ? sessionParams.rightOffsetCentimeters : sessionParams.leftOffsetCentimeters) / 100;
        RepositionCube( (float) sessionParams.spawnHeightDecimetres / 10, sessionParams.spawningDistanceMetres );
        Debug.Log(_targetSide);
    }

    private void RepositionCube(float originY, float originZ)
    {
        var startingX = _isStanding ? Camera.main.transform.position.x : meanPosition.value.x;
        var offsetDirection = _targetSide == TargetSide.LEFT ? 1 : -1;
        transform.position = new Vector3( startingX + offsetDirection * _xOffset, 0, 0)
            + new Vector3(0, originY, originZ);
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
        yield return new WaitForSecondsRealtime(10f);
        Destroy(gameObject);
    }

    void Update()
    {
        if (_hasHitPlayer || isSessionPaused.value == true) return;
        transform.position = GetUpdatedPosition();
    }

    private Vector3 GetUpdatedPosition()
    {
        var delta = _speed* Time.deltaTime * -transform.forward;
        var newPos = transform.position + delta;
        if (_isStanding && transform.position.z > _zThresholdForStoppingModification) newPos.x = GetOffsetedX();
        return newPos;

    }

    private float GetOffsetedX()
    {
        var offsetDirection = _targetSide == TargetSide.LEFT ? 1 : -1;
        return Camera.main.transform.position.x + offsetDirection * _xOffset;
    }

    public void OnCubeHitPlayer()
    {
        Debug.Log("Hit");
        _hasHitPlayer = true;
        OnCubeCollidedObject?.Invoke(this.gameObject);
        OnCubeCollided?.Invoke(_targetSide, EventType.HIT);
        StartCoroutine(DestroyCube());
    }

    public void OnCubeHitWall()
    {

        Debug.Log("Dodged");
        OnCubeCollidedObject?.Invoke(this.gameObject);
        OnCubeCollided?.Invoke(_targetSide, EventType.DODGED);
        StartCoroutine(DestroyCube());

    }

}
public enum TargetSide
{
    LEFT, RIGHT
}

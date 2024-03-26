using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubeModel;
    [SerializeField] private Vector3Data meanPosition;
    private TargetSide _targetSide;
    public bool isStanding;
    private float _currrentScaleFactor;
    private int _speed;

    public void InitiateCube(SessionParams sessionParams, TargetSide targetSide, Vector3 originPosition)
    {
        StartCoroutine(DestroyCube());
        _targetSide = targetSide;
        isStanding = sessionParams.isStanding;
        _speed = sessionParams.speed;
        _currrentScaleFactor = RescaleCube(sessionParams.cubeScaleDecimeters);
        RepositionCube(originPosition, targetSide == TargetSide.LEFT ? sessionParams.rightOffsetCentimeters : -sessionParams.leftOffsetCentimeters);
        Debug.Log("*************Velocity: " + new Vector3(0, 0, -sessionParams.speed));
    }

    private void RepositionCube(Vector3 originPosition, int XOffsetInCentimeters)
    {
        transform.position = new Vector3(meanPosition.value.x + (float)XOffsetInCentimeters / 100, 0, 0)
            + new Vector3(0, originPosition.y, originPosition.z);
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
        transform.position += _speed * Time.deltaTime * -transform.forward;
    }

    private void OnCubeHitPlayer()
    {

    }

    private void OnCubeHitWall()
    {

    }
}

public enum TargetSide
{
    LEFT, RIGHT
}
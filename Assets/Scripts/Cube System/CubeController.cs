using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject cubeModel;
    private float _currrentScaleFactor;
    public void InitiateCube(SessionParams sessionParams, TargetSide targetSide, Vector3 originPosition)
    {
        StartCoroutine(DestroyCube());
        Debug.Log("*************Target side: " + targetSide);
        _currrentScaleFactor = RescaleCube(sessionParams.cubeScaleDecimeters);
        RepositionCube(originPosition, targetSide== TargetSide.LEFT ? sessionParams.rightOffsetCentimeters : -sessionParams.leftOffsetCentimeters);
    }

    private void RepositionCube(Vector3 originPosition, int XOffsetInCentimeters)
    {
        transform.position = originPosition + new Vector3((float)XOffsetInCentimeters/100,0,0);
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
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
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
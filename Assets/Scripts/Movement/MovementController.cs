using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
        var pos = _camera.transform.position;
        pos.y = transform.position.y;
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);

    }

    private void Update()
    {
        var pos = _camera.transform.position;
        pos.y = transform.position.y;
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }
}

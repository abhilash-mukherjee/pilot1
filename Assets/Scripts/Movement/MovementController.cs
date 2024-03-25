using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
        transform.SetLocalPositionAndRotation(new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z), Quaternion.identity);

    }

    private void Update()
    {
        transform.SetLocalPositionAndRotation(new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z), Quaternion.identity);
    }
}

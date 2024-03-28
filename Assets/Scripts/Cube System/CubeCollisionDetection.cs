using System;
using UnityEngine;

public class CubeCollisionDetection : MonoBehaviour
{
    [SerializeField] private string playerTag, behindWallTag, uiWallTag;
    [SerializeField] private CubeController cubeController;
    private bool _hasHitPlayer = false;

    private void OnEnable()
    {
        SessionManager.OnSessionEnded += DisableColliders;
    }
    
    private void OnDisable()
    {
        SessionManager.OnSessionEnded -= DisableColliders;
    }

    private void DisableColliders(SessionData data)
    {
        Destroy(GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _hasHitPlayer = true;
            GetComponent<Collider>().isTrigger = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = new Vector3(0, 0, -10);
            cubeController.OnCubeHitPlayer();
        }

        if (other.CompareTag(behindWallTag))
        {
            if (!_hasHitPlayer) cubeController.OnCubeHitWall();
        }
    }
}

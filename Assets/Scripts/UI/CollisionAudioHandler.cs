using UnityEngine;

public class CollisionAudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource hitSource, dodgeSource;
    private void OnEnable()
    {
        CubeController.OnCubeCollided += PlayAudio;
    }

    private void OnDisable()
    {
        CubeController.OnCubeCollided -= PlayAudio;
    }

    private void PlayAudio(TargetSide side, EventType eventType)
    {
        if (eventType == EventType.HIT)
        {
            hitSource.Play();
        }
        else dodgeSource.Play();
    }
}
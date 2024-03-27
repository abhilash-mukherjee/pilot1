using UnityEngine;

public class CubeSpawnManager : MonoBehaviour
{
    [SerializeField] CubeController cubeController;
    private int spawnDelay = 3;
    private TargetSide _lastTargetedSide = TargetSide.LEFT;
    private void OnEnable()
    {
        TimerManager.OnTimerTicked += TimerTicked;
    }

    private void OnDisable()
    {
        TimerManager.OnTimerTicked -= TimerTicked;
    }

    private void TimerTicked(SessionParams sessionParams, int timeLeft)
    {
        var totalDuration = sessionParams.duration;
        var timeElapsed = totalDuration - timeLeft;
        if ((timeElapsed - spawnDelay) % sessionParams.cubeGap == 0)
        {
            SpawnCube(sessionParams);
        }

    }

    private void SpawnCube(SessionParams sessionParams)
    {
        var spawnnedCube = Instantiate(cubeController, transform.position, Quaternion.identity);
        spawnnedCube.InitiateCube(sessionParams, GettargetSide(sessionParams));
    }

    private TargetSide GettargetSide(SessionParams sessionParams)
    {
        if (sessionParams.targetSide == "LEFT")
        {
            _lastTargetedSide = TargetSide.LEFT;
            return TargetSide.LEFT;
        }
        else if (sessionParams.targetSide == "RIGHT")
        {
            _lastTargetedSide = TargetSide.RIGHT;
            return TargetSide.RIGHT;
        }
        else
        {
            if (_lastTargetedSide == TargetSide.LEFT)
            {
                _lastTargetedSide = TargetSide.RIGHT;
                return TargetSide.RIGHT;
            }
            else
            {
                _lastTargetedSide = TargetSide.LEFT;
                return TargetSide.LEFT;
            }
        }
    }
}

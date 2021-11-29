using System.Collections;
using System.Collections.Generic;
using Game_Managing.Pausing;
using Other;
using UnityEngine;

public abstract class Pausable : MonoBehaviour
{
    [SerializeField] private PauseTarget _pauseTarget;

    enum PauseTarget
    {
        OnlyTruePause, Any
    }

    // Update is called once per frame
    protected virtual void PausableUpdate()
    {
    }

    bool shouldPause(Optional<PauseManager.PauseType> pauseType)
    {
        if (!pauseType.Enabled)
        {
            return false;
        }

        if (_pauseTarget == PauseTarget.OnlyTruePause)
        {
            return pauseType.Value == PauseManager.PauseType.All;
        }

        return true;
    }


    void Update()
    {
        if (!shouldPause(PauseManager.Instance.Paused)) PausableUpdate();
    }
}

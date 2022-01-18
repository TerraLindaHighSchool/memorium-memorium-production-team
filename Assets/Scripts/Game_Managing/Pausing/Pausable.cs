using System.Collections;
using System.Collections.Generic;
using Game_Managing.Pausing;
using Other;
using UnityEngine;

/// <summary>
/// A class that exposes the PausableUpdate method, which should be used over Update in inheriting classes.
/// This method will only be run when the game is not paused.
/// </summary>
public abstract class Pausable : MonoBehaviour
{
    [SerializeField] private PauseTarget _pauseTarget;

    /// <summary>
    /// If the inheriting class's PausableUpdate method should be paused during any pause or only a "true" pause (not cutscene)
    ///
    /// <c>OnlyTruePause</c> -> things that still process during cutscenes
    /// <c>Any</c> -> everything else except for things that don't pause, like global managers.
    /// </summary>
    enum PauseTarget
    {
        OnlyTruePause, Any
    }

    /// called once per frame unless the game is paused with the correct PauseType
    protected virtual void PausableUpdate()
    {
    }

    /// <summary>
    /// Determines if this component should pause given the global pause state.
    /// </summary>
    /// <param name="pauseType">Always the global pause state to be matched against this components's PauseTarget</param>
    /// <returns>if this class should pause.</returns>
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

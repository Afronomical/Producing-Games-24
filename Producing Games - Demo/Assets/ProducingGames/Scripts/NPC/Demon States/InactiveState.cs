using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// When the rage mode isn't happening and the demon has been instantiated
/// </summary>

public class InactiveState : DemonStateBaseClass
{
    // INFO: Need to implement logic for demon to go into inactive state
    // If ragemode doesn't occur we can set the demon to inactive
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateLogic()
    {
        // INFO: Need some logic that tells us when rage mode is activate so that
        // we can re-activate the demon if it's already been spawned in previously
        /*
         * if (GameManager.Instance.IsRageModeActivated)
         * {
         *      gameObject.SetActive(true);
         *      character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
         * }
         */
    }

    private void OnDestroy()
    {
        gameObject.SetActive(true);
    }
}

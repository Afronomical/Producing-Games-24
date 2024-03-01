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

    /*public override void UpdateLogic()
    {
    }*/
}

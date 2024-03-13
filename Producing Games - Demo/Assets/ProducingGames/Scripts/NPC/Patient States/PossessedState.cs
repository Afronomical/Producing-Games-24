using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Handles the logic of the possessed patient
/// </summary>
public class PossessedState : PatientStateBaseClass
{
    private readonly float possessedDuration = 5.0f;

    private void Start()
    {
        DemonCharacter demonCharacter = GameManager.Instance.demon.GetComponent<DemonCharacter>();

        // INFO: Makes the demon start patrolling and spawns it at the possessed patients bed
        demonCharacter.gameObject.SetActive(true);
        demonCharacter.agent.Warp(character.bed.transform.position);

        // PLAY POSSESSED ANIMATION HERE

        Invoke(nameof(LeavePossessedState), possessedDuration);
    }

    private void LeavePossessedState()
    {
        UnityEngine.Debug.Log("Left possessed state");
        character.ChangePatientState(character.PreviousState);
    }
}

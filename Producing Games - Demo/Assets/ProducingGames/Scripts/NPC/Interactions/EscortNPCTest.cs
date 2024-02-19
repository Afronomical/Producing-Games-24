using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EscortNPCTest : NPCInteractableTemplate
{

    public override void Escort()
    {
        character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
        Debug.Log("changed into escort stage");
    }
}

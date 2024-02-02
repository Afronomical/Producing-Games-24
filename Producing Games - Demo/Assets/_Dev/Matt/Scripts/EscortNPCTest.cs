using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EscortNPCTest : NPCInteractableTemplate
{

    public override void Escort()
    {
        character.ChangeState(AICharacter.States.Escorted);
        Debug.Log("changed into escort stage");
    }
}

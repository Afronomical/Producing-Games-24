using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeValve : InteractableTemplate
{
    public InteractiveObject turnOnSO, turnOffSO;
    public bool waterOn;
    [HideInInspector] public PipeMinigame puzzle;

    public override void Interact()
    {
        if (waterOn == true)
        {
            waterOn = false;
            collectible = turnOnSO;
            GetComponent<BoxCollider>().enabled = false;
            puzzle.GetComponent<BoxCollider>().enabled = true;
        }
        else if (puzzle.complete)
        {
            waterOn = true;
            collectible = turnOffSO;
            GetComponent<BoxCollider>().enabled = false;
            PatientTaskManager.instance.CheckTaskConditions(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{

    InputAction.CallbackContext c;
    public PlayerArms arms;
    [HideInInspector] public bool canPickUp = true;
    [HideInInspector] public InspectableObject currentlyInspecting;
    [HideInInspector] public HidingCutScene currentLocker;

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            c = context;
            //if(PlayerInteractor.instance.currentObject != null) 
            //{
            //    PlayerInteractor.instance.currentObject.Interact();
            
            //}
        }
    }

    public void OnStopInspectingInput(InputAction.CallbackContext context)
    {
        if (currentlyInspecting != null)
        {
            currentlyInspecting.StopInspecting();
            currentlyInspecting = null;
        }
        if (currentLocker != null)
        {
            currentLocker.ExitInput();
            currentLocker = null;
        }
    }

    private void Update()
    {
        Ray r = new Ray(PlayerInteractor.instance.interactorSource.position, PlayerInteractor.instance.interactorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hit, PlayerInteractor.instance.interactionRange) && GetComponent<PlayerInput>().enabled)
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                PlayerInteractor.instance.currentObject = interactable;

                //checks if object is interactable
                if (interactable is InteractableTemplate interactableTemplate && interactableTemplate.hasBeenPlaced == false)
                {
                    if (interactableTemplate.gameObject != null)
                        PatientTaskManager.instance.DetectTasks(interactableTemplate.gameObject);

                    if (interactableTemplate.collectible != PatientTaskManager.instance.noTaskPrompt && interactableTemplate.collectible != null)
                    {
                        //shows tooltip on mouse hover
                        TooltipManager.Instance.ShowTooltip(interactableTemplate.collectible.tooltipText, interactableTemplate.collectible.tooltipImage);

                        if (c.performed && canPickUp)
                        {
                            if (interactableTemplate.gameObject != null)
                                PatientTaskManager.instance.CheckTaskConditions(interactableTemplate.gameObject);

                            if (interactableTemplate.UIToDisable != null)
                            {
                                for (int i = 0; i < interactableTemplate.UIToDisable.Length; i++)
                                {
                                    interactableTemplate.UIToDisable[i].SetActive(false);
                                }
                            }

                            if (PlayerInteractor.instance.currentObject != null)
                            {
                                //Debug.Log("Interact");

                                StartCoroutine(arms.GrabObject(interactableTemplate.collectible));
                            }
                            
                        }
                    }
                }
            }

            else TooltipManager.Instance.HideTooltip();

            if (hit.collider.gameObject.TryGetComponent(out INPCInteractable NPCInteractable))
            {
                PlayerInteractor.instance.currentNPC = NPCInteractable;
                if (NPCInteractable is NPCInteractableTemplate NPCTemplate)
                {
                    // INFO: If not in any of the states that allow the patient to be escorted
                    // then we will return
                    if (NPCTemplate.character.currentState is 
                        not PatientCharacter.PatientStates.Abandoned and
                        not PatientCharacter.PatientStates.Hiding and
                        not PatientCharacter.PatientStates.Wandering and
                        not PatientCharacter.PatientStates.Hungry and
                        not PatientCharacter.PatientStates.Prayer)
                        return;

                    if (c.performed)
                    {
                        NPCTemplate.character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
                    }
                }
               
            }
        }
        else
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}

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

    private void Update()
    {
        Ray r = new Ray(PlayerInteractor.instance.interactorSource.position, PlayerInteractor.instance.interactorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hit, PlayerInteractor.instance.interactionRange) && GetComponent<PlayerInput>().enabled)
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                PlayerInteractor.instance.currentObject = interactable;

                //checks if object is interactable
                if (interactable is InteractableTemplate interactableTemplate)
                {
                    if (interactableTemplate.gameObject != null)
                        PatientTaskManager.instance.DetectTasks(interactableTemplate.gameObject);

                    if (interactableTemplate.collectible != PatientTaskManager.instance.noTaskPrompt && interactableTemplate.collectible != null)
                    {
                        //shows tooltip on mouse hover
                        TooltipManager.Instance.ShowTooltip(interactableTemplate.collectible.tooltipText);

                        if (c.performed && canPickUp)
                        {
                            if (interactableTemplate.gameObject != null)
                                PatientTaskManager.instance.CheckTaskConditions(interactableTemplate.gameObject);

                            if (PlayerInteractor.instance.currentObject != null)
                            {
                                Debug.Log("Interact");
                                StartCoroutine(arms.GrabObject());
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
                    if (NPCTemplate.character.currentState != PatientCharacter.PatientStates.Bed &&  // If not in any of these states
                        NPCTemplate.character.currentState != PatientCharacter.PatientStates.Dead &&
                        NPCTemplate.character.currentState != PatientCharacter.PatientStates.Escorted &&
                        NPCTemplate.character.currentState != PatientCharacter.PatientStates.Possessed &&
                        NPCTemplate.character.currentState != PatientCharacter.PatientStates.ReqMeds)
                    {
                        if (c.performed)
                        {
                            NPCTemplate.character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
                            //currentNPC.Escort();
                        }
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

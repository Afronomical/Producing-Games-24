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
                    //shows tooltip on mouse hover
                    TooltipManager.Instance.ShowTooltip(interactableTemplate.collectible.tooltipText);
                    PatientTaskManager.instance.DetectTasks(interactableTemplate.gameObject);

                    if (c.performed && canPickUp)
                    {
                        PatientTaskManager.instance.CheckTaskConditions(interactableTemplate.gameObject);

                        if (PlayerInteractor.instance.currentObject != null)
                        {
                            Debug.Log("Interact");
                            StartCoroutine(arms.GrabObject());
                        }
                    }
                }
            }
            else if (hit.collider.gameObject.TryGetComponent(out INPCInteractable NPCInteractable))
            {
                PlayerInteractor.instance.currentNPC = NPCInteractable;
                if (NPCInteractable is NPCInteractableTemplate NPCTemplate)
                {
                    PatientTaskManager.instance.DetectTasks(NPCTemplate.character.gameObject);

                    if (c.performed)
                        PatientTaskManager.instance.CheckTaskConditions(NPCTemplate.character.gameObject);

                    if (NPCTemplate.character.currentState == PatientCharacter.PatientStates.Wandering)
                    {
                        TooltipManager.Instance.ShowTooltip("ESCORT " + NPCTemplate.ToolTipText);
                        if (c.performed)
                        {
                            NPCTemplate.character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
                            //currentNPC.Escort();
                        }
                    }
                    else if (NPCTemplate.character.currentState == PatientCharacter.PatientStates.Bed)
                    {
                        ////can interact unless dead 

                    }


                }
            }
            else
            {
                TooltipManager.Instance.HideTooltip();

            }
        }
        else
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}

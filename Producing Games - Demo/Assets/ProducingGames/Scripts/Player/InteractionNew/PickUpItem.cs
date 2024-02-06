using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{

    InputAction.CallbackContext c;

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

        if (Physics.Raycast(r, out RaycastHit hit, PlayerInteractor.instance.interactionRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                PlayerInteractor.instance.currentObject = interactable;

                //checks if object is interactable
                if (interactable is InteractableTemplate interactableTemplate)
                {
                    //shows tooltip on mouse hover
                    TooltipManager.Instance.ShowTooltip(interactableTemplate.collectible.tooltipText);

                    if (c.performed)
                    {
                        if (PlayerInteractor.instance.currentObject != null)
                        {
                            PlayerInteractor.instance.currentObject.Interact();

                        }
                    }
                }
            }
            else if (hit.collider.gameObject.TryGetComponent(out INPCInteractable NPCInteractable))
            {
                PlayerInteractor.instance.currentNPC = NPCInteractable;
                if (NPCInteractable is NPCInteractableTemplate NPCTemplate)
                {
                    if (c.performed)
                        PatientTaskManager.instance.CheckTaskConditions(NPCTemplate.character.gameObject);

                    if (NPCTemplate.character.currentState == AICharacter.States.Wandering)
                    {
                        TooltipManager.Instance.ShowTooltip("ESCORT " + NPCTemplate.ToolTipText);
                        if (c.performed)
                        {
                            NPCTemplate.character.ChangeState(AICharacter.States.Escorted);
                            //currentNPC.Escort();
                        }
                    }
                    else if (NPCTemplate.character.currentState == AICharacter.States.Bed)
                    {
                        ////can interact unless dead 

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

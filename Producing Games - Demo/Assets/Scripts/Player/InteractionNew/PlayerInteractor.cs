using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IInteractable
{
    public string Name { get; }
    public void Interact();
    public void Collect();

}
public class PlayerInteractor : MonoBehaviour
{
    public Transform interactorSource;

    public float interactionRange;

    private IInteractable currentObject;

    private Outline outline;

    private void Start()
    {
        //interactableTemplate.gameObject.GetComponent<Outline>().enabled = false;
    }
    private void Update()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        if(Physics.Raycast(r, out RaycastHit hit, interactionRange))
        {
            if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                currentObject = interactable;

                //checks if object is interactable
                if(interactable is InteractableTemplate interactableTemplate) 
                {

                    TooltipManager.Instance.ShowTooltip(interactableTemplate.collectible.tooltipText + " " + interactableTemplate.collectible.objectName);
                    ////checks what type of interactable object it is
                    //switch (interactableTemplate.type)
                    //{
                    //    case InteractableTemplate.ObjectType.Cube:
                    //        Debug.Log("Interacted with cube");
                    //        break;
                    //    case InteractableTemplate.ObjectType.Stick:
                    //        Debug.Log("Interacted with stick");
                    //        break;
                    //    case InteractableTemplate.ObjectType.Paper:
                    //        Debug.Log("Interacted with paper");
                    //        break;
                    //    default:
                    //        break;
                    //}
                    
                    outline = interactableTemplate.gameObject.GetComponent<Outline>();
                    if(outline != null)
                    {
                        interactableTemplate.gameObject.GetComponent<Outline>().enabled = true;

                    }
                }
            }
        }
        else
        {
            TooltipManager.Instance.HideTooltip();
            if(outline != null)
            {
                outline.enabled = false;

            }
        }
    }
}

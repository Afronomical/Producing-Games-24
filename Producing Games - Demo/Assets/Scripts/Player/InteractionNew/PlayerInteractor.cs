using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

                    if(Input.GetMouseButtonDown(0))
                    {
                        currentObject.Interact();

                    }
                    
                    outline = interactableTemplate.gameObject.GetComponent<Outline>();
                    if(outline != null)
                    {
                        interactableTemplate.gameObject.GetComponent<Outline>().enabled = true;

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
        else
        {
            TooltipManager.Instance.HideTooltip();
            if (outline != null)
            {
                outline.enabled = false;

            }
        }
    }
}

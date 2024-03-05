using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///<para>Written By: Matthew Brake </para>
///Moderated By:......
/// <para>Creates slots and storage spaces for exorcism items within the chest, and contains items dropped into it </para>
/// </summary>
public class ExorcismChest : MonoBehaviour
{

    public static ExorcismChest instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }



    [Header("Chest Stats")]
    [Space]
    public int maxSlots;
    public int takenSlots; 
    public List<InteractableTemplate> currentItems { get; private set; }
    private float radius = 2f;
    public SoundEffect dropSound;
    public SoundEffect pickUpSound; 

    


    private void Start()
    {
        
    }

    public void AddtoChest()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            //check they are interactable objects e.g water, cross, before adding to the count 
            if (collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                if (collider.gameObject.GetComponent<InteractableTemplate>().isExorcismObject)
                {
                    if (collider.gameObject.GetComponent<InteractableTemplate>().hasBeenPlaced == false)
                    {
                        TooltipManager.Instance.ShowTooltip("Press C to Add to Chest");
                        if (Input.GetKeyUp(KeyCode.C))
                        {
                            ///destroy object and add it to the inventory list 
                        }
                    }


                 }
            }
        }
    }

}

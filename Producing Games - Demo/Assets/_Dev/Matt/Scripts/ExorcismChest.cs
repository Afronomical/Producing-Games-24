using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///<para>Written By: Matthew Brake </para>
///Moderated By:......
/// <para>Creates slots and storage spaces for exorcism items within the chest, and contains items dropped into it </para>
/// </summary>
public class ExorcismChest : InteractableTemplate
{

    [Header("Chest Stats")]
    [Space]
    [Tooltip("The maximum amount of items which can be placed ")]
    public int maxSlots;
    [SerializeField] private int takenSlots = 0;
    private int availableSlots;
    private bool chestOpen = false;
    private bool canDrop = true;
    private bool isInspecting = false;
    public List<GameObject> currentItems = new();


    [Tooltip("Define points inside the chest where tooltips can sit")]
    public List<Transform> dropPoints = new();
    private float radius = 2f;
    public SoundEffect dropSound;
    public SoundEffect pickUpSound;
    private Animator animator;


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



    

    


    private void Start()
    {
        availableSlots = maxSlots;
        collectible.tooltipText = "Interact With Chest";
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AddtoChest();
    }

    public override void Interact()
    {
        if(!chestOpen)
        {
            animator.speed = 1;
            animator.SetTrigger("OpenedChest");

           // Debug.Log("Chest opening");
            chestOpen = true;
            collectible.tooltipText = "Press C to Inspect Objects";
        }
        else if(chestOpen)
        {
            animator.SetTrigger("CloseChest");
            Debug.Log("Chest closing");
            collectible.tooltipText = "Interact With Chest";
            foreach(var item in currentItems)
            {
                item.GetComponent<InteractableTemplate>().hasBeenPlaced = true;
                Debug.Log("Set item to placed");
            }

            chestOpen = false;
        }
    }

    public void AddtoChest()
    {
       if(chestOpen && availableSlots > 1)
        {
            Debug.Log("Got past chest is open");
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders)
            {
                //check they are interactable objects e.g water, cross, before adding to the count 
                if (collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    if (collider.gameObject.GetComponent<InteractableTemplate>().isExorcismObject)
                    {
                        Debug.Log("is an exorcism item");
                        if (collider.gameObject.GetComponent<InteractableTemplate>().hasBeenPlaced == false)
                        {
                            currentItems.Add(collider.gameObject);
                            --availableSlots;
                            int dropLoc = Random.Range(0, dropPoints.Count);
                            collider.gameObject.transform.position = dropPoints[dropLoc].transform.position;
                            collider.gameObject.transform.rotation = Quaternion.identity;
                            dropPoints.RemoveAt(dropLoc);
                            Debug.Log("added: " + collider.gameObject.name);
                        }


                    }
                }
            }
        }
       
    }

    [ContextMenu("Fill Chest")]
    public void FillChest()
    {
        for(int i = 0; i < maxSlots; i++)
        {

        }
    }

    public void RemoveFromChest()
    {

    }

    public void Inspect()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
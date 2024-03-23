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
    private bool isMoving = false;
    private bool stopInspecing = false;
    public List<GameObject> currentItems = new();
    private float t;
    private float returnT;
    private float currentTime = 0f;
    public float timeToInspect = 3f;
    private float timeToMove = 3f;
    public float panToChestSpeed = 0.05f;
    private Camera mainCam;
    public Transform inspectPoint;
    private Transform originalTransform;
    private Quaternion originalPlayerRot;
    private GameObject player;
    private CameraLook cameraLook;

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
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
        cameraLook = mainCam.GetComponent<CameraLook>();
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }

    private void Update()
    {
        AddtoChest();
        t = currentTime / timeToMove;
       
        if (isInspecting)
        {
            GetComponent<BoxCollider>().enabled = false;
            currentTime += Time.deltaTime;
            Debug.Log(currentTime);
            if (isMoving)
            {
               
                
                Vector3 newPos = player.transform.position - inspectPoint.position;
                player.transform.position = Vector3.Lerp(player.transform.position, inspectPoint.position, t * panToChestSpeed);  

                Quaternion targetRot = Quaternion.LookRotation(transform.position - player.transform.position);  //works but angle is off 
                float reducedXAngle = targetRot.eulerAngles.x * 0.8f;
                Quaternion adjustedRotation = Quaternion.Euler(reducedXAngle, targetRot.eulerAngles.y, targetRot.eulerAngles.z);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, adjustedRotation, t * panToChestSpeed);
                player.GetComponent<PlayerMovement>().enabled = false;
                cameraLook.enabled = false;
                //if (player.transform.position == newPos)
                //{
                    
                //    isMoving = false;
                    
                //}
            }
            if (currentTime >= timeToInspect)
            {
                isInspecting = false;
                isMoving = false;
                returnT = t;
                Return();
            }
        }
      
    }

    public override void Interact()
    {
        originalTransform = player.transform;
        originalPlayerRot = player.transform.rotation;

       
         
            if (!chestOpen)
            {
                animator.speed = 1;
                animator.SetTrigger("OpenedChest");
               

            // Debug.Log("Chest opening");
                chestOpen = true;
            ///need to move to new input system to allow for inspection 
                currentTime = 0;
                isInspecting = true;
                isMoving = true;
            }
            else if (chestOpen)
            {
                animator.SetTrigger("CloseChest");
              
                Debug.Log("Chest closing");
                foreach (var item in currentItems)
                {
                    item.GetComponent<InteractableTemplate>().hasBeenPlaced = true;
                    Debug.Log("Set item to placed");
                }
               

                chestOpen = false;
            }
        
       
       
    }

     private void Return()
    {
        Debug.Log("returning");
        isInspecting = false;
        isMoving = false;
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, originalPlayerRot, 1);
        player.transform.position = Vector3.Lerp(player.transform.position, originalTransform.position,1);
      
            player.GetComponent<PlayerMovement>().enabled = true;
            cameraLook.enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        
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
                if (collider.gameObject.TryGetComponent(out IInteractable interactable) && collider.gameObject.GetComponent<InteractableTemplate>().isExorcismObject && collider.gameObject.GetComponent<InteractableTemplate>().hasBeenPlaced == false)
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

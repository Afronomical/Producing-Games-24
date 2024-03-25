using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: ...... </para>
/// <para> Manages the interaction of the organ to enable use of the Exorcism Table. </para>
/// </summary>
public class OrganTrigger : InteractableTemplate
{
   
    private float currentTime = 0f;
    private float timeToMove = 4f;
    public float panToTableSpeed = 0.05f;
    private bool isPanning;
    private bool enabledTable;
    private bool needToInteract;
    private ExorcismTable exorcismTable;
    public GameObject panPosition;
    private GameObject player;
    private Camera mainCam;
    private CameraLook cameraLook;
    private Quaternion originalPlayerRot;
    private float t;
    private float returnT;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        isPanning = false;
        enabledTable = false;
        exorcismTable = FindFirstObjectByType<ExorcismTable>(); 
        mainCam = Camera.main;
        cameraLook = mainCam.GetComponent<CameraLook>();
        needToInteract = false;
        

    }

    private void Update()
    {
        t = currentTime / timeToMove;
        
        if (needToInteract)
        {
            currentTime += Time.deltaTime;
            Debug.Log("Past need to interact");
            if (isPanning)
            {
                
                
                cameraLook.enabled = false;
                Debug.Log("Panning"); ////gets here but does none of the after 


                Quaternion targetRot = Quaternion.LookRotation(panPosition.transform.position - player.transform.position);
                Debug.Log("Target rot is: " + targetRot.eulerAngles);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, t * panToTableSpeed);
               

                if (currentTime >= timeToMove)
                {
                    isPanning = false;
                    returnT = t;

                    // player.transform.rotation = Quaternion.identity;
                    currentTime = 0f;

                }

            }
            else
            {
                Invoke(nameof(Return), 2f);
               

            }
        }

       
       
    }

    public override void Interact()
    {
        ///begin animation 
        if(!enabledTable)
        {
            enabledTable = true;
            needToInteract = true;
            //StartCoroutine(EnableExorcismDelay(3f/*until animation ends*/));
            // isPanning = true;
            PanToTable();
            originalPlayerRot = player.transform.rotation;
            
        }
        
        
        
    }



    private void PanToTable()
    {

        currentTime = 0;
        isPanning = true;
        exorcismTable.tableAvailable = true;

    }

    private void Return()
    {
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, originalPlayerRot, (t - returnT) * panToTableSpeed);
        if (player.transform.rotation == originalPlayerRot)
        {
            cameraLook.enabled = true;
            needToInteract = false;
            
        }
    }
    //private void OnMouseEnter()
    //{
    //    if(!needToInteract)
    //    {
    //        TooltipManager.Instance.HideTooltip();
    //    }
    //}

    
}

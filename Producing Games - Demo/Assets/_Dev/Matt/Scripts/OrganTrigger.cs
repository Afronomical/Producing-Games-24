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
    public Vector3 targetRot;
    private Camera cam;
    private float currentTime = 0f;
    private float timeToMove = 3f;
    private bool isPanning;
    private ExorcismTable exorcismTable;

    private void Start()
    {
       isPanning = false;
       cam = Camera.main;
        targetRot = GameManager.Instance.altar.transform.position;
        exorcismTable = FindFirstObjectByType<ExorcismTable>(); 
    }

    private void Update()
    {
        if(isPanning)
        {
            
            Debug.Log("Panning"); ////gets here but does none of the after 
           
        }
    }

    public override void Interact()
    {
        ///begin animation 
        StartCoroutine(EnableExorcismDelay(3f/*until animation ends*/));
        
    }

    public IEnumerator EnableExorcismDelay(float seconds)
    {
        ///wait until animation finished before panning camera to the table for exorcism to take place 
        //Debug.Log("beginning coroutine");
        yield return new WaitForSeconds(seconds);
        PanToTable();
    }


    private void PanToTable()
    {
        
        currentTime = 0;
        isPanning=true;
        exorcismTable.tableAvailable = true;
    }
}

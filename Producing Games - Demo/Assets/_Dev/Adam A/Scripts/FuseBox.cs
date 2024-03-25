using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class FuseBox : InspectableObject
{
    public enum Colours { Red, Blue, Yellow, Purple };
     
    public GameObject cursor;
    
    
    public List<GameObject> fuses, fuseSlots;//Fuses/FuseSlots references
    public Material[] materials;//All the Colour Materials
    private int heldFuses;//Correct Fuses
    public bool complete;//If this minigame is completed

    //Materials for the lights
    [Header("Correct/Incorrect Config")]
    public Material correctMaterial;
    public Material incorrectMaterial;
    public Material correctOffMaterial;
    public Material incorrectOffMaterial;
    
    //Lights Object Reference
    public GameObject correctObject;
    public GameObject incorrectObject;


   
    public override void Interact()
    {
        base.Interact();
        Cursor.lockState = CursorLockMode.None;
        ToggleBoxCollider(false);
        cursor.SetActive(true);
    }
    protected override void Start()
    {
        ToggleBoxCollider(true);
        InitializeBox();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking)
        {
            ToggleBoxCollider(false);
            StopUsing();
            if (!complete)
            {
                ToggleBoxCollider(true);
            }
        }

    }

    //This will randomly swap the colours that the fuses will need to go into, and take the fuses out and put them onto the side
    public void InitializeBox()
    {
        complete = false;
        LightManager.Instance.AllLightToggle(false);
        correctObject.GetComponent<Renderer>().material = correctOffMaterial;
        incorrectObject.GetComponent<Renderer>().material = incorrectMaterial;
        ToggleBoxCollider(true);

        List<GameObject> w = new List<GameObject>(fuses);
        List<GameObject> e = new List<GameObject>(fuseSlots);
        for (int i = 3; i >= 0; i--)
        {
            int rand = Random.Range(0, w.Count);
            Fuse currentFuse = w[rand].GetComponent<Fuse>();
            currentFuse.colour = (Colours)i;
            w[rand].GetComponent<Renderer>().material = materials[i];
            w.Remove(w[rand]);

            rand = Random.Range(0, e.Count);
            e[rand].GetComponent<Renderer>().material = materials[i];
            currentFuse.fuseSlot = e[rand];
            e.Remove(e[rand]);
        }

        foreach (GameObject obj in fuses)
        {
            obj.GetComponent<Fuse>().InitializeFuse();
        }
    }

    //If the fuse isn't connected to it's correct slot, you can hold onto the fuse and move it around
    public void Select()
    {
        foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)//If the cursor is colliding with objects
        {
            Debug.Log(obj.name);
            Debug.Log(obj.GetComponent<Fuse>().connected);
            if (fuses.Contains(obj) && !obj.GetComponent<Fuse>().connected)//If the fuse is within the list of fuses and isn't connected
            {
                Debug.Log(obj);
                heldFuses = fuses.IndexOf(obj);//Grabs reference to the selected fuse
                obj.GetComponent<Fuse>().Hold();//Hold the fuse
                break;
            }
        }
    }

    //If the fuse is over the correct slot, connect the fuse to that slot, if all 4 fuses are connected, the task is complete
    public void Release()
    {
        
        if (heldFuses != -1)//Checks if the player is holding a valid fuse(Without this the fuse will be stuck to the mouse)
        {
            if (cursor.GetComponent<MinigameCursor>().collidingObjects.Count == 0)//If cursor is colliding with 0 objects
            {
                fuses[heldFuses].GetComponent<Fuse>().Release();//The fuse that is being held will be released
                heldFuses = -1;//Resets the "Held Fuse" reference 
            }

            else
            {
                foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)//Each object the cursor is colliding with
                {
                    if (fuseSlots.Contains(obj) && fuses[heldFuses].GetComponent<Fuse>().fuseSlot == obj)//Validates if it's the correct slot
                    {
                        fuses[heldFuses].GetComponent<Fuse>().Connect();//Connect the fuse to the slot
                        heldFuses = -1;//Resets the "Held Fuse" reference 

                        int connectedFuses = 0;
                        foreach (GameObject fuse in fuses)  // Check all fuses
                        {
                            if (fuse.GetComponent<Fuse>().connected) ++connectedFuses;  // If they are all connected
                        }

                        if (connectedFuses >= 4)//If all fuses are connected, Complete the Task, Turn Light's on and exit the player out of the minigame
                        {
                            complete = true;
                            LightManager.Instance.AllLightToggle(true);
                            correctObject.GetComponent<Renderer>().material = correctMaterial;
                            incorrectObject.GetComponent<Renderer>().material = incorrectOffMaterial;
                            ToggleBoxCollider(false);
                            PatientTaskManager.instance.CheckTaskConditions(gameObject);
                            StopUsing();
                        }

                        break;
                    }

                    else  //If the held fuse isn't over the correct slot, release the fuse
                    {
                        fuses[heldFuses].GetComponent<Fuse>().Release();
                    }
                }
            }
        }
    }

    private void ToggleBoxCollider(bool enable)
    {
        GetComponent<BoxCollider>().enabled = enable;
    }

    //Exits the player out of the minigame
    private void StopUsing()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(false);
        looking = false;
        playerCanMove = true;
        stopLooking = true;
    }
}

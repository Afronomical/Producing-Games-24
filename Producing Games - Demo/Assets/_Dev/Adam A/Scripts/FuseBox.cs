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
    private int activeFuses;//Correct Fuses
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
        GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        cursor.SetActive(true);
    }
    protected override void Start()
    {
        InitializeBox();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking) StopUsing();
    }

    //This will randomly swap the colours that the fuses will need to go into, and take the fuses out and put them onto the side
    public void InitializeBox()
    {
        complete = false;
        LightManager.Instance.AllLightToggle(false);
        correctObject.GetComponent<Renderer>().material = correctOffMaterial;
        incorrectObject.GetComponent<Renderer>().material = incorrectMaterial;
        GetComponent<BoxCollider>().enabled = true;

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
        foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
        {
            Debug.Log(obj.name);
            Debug.Log(obj.GetComponent<Fuse>().connected);
            if (fuses.Contains(obj) && !obj.GetComponent<Fuse>().connected)
            {
                Debug.Log(obj);
                activeFuses = fuses.IndexOf(obj);
                obj.GetComponent<Fuse>().Hold();
                break;
            }
        }
    }

    //If the fuse is over the correct slot, connect the fuse to that slot, if all 4 fuses are connected, the task is complete
    public void Release()
    {
        if (activeFuses != -1)
        {
            if (cursor.GetComponent<MinigameCursor>().collidingObjects.Count == 0)
            {
                fuses[activeFuses].GetComponent<Fuse>().Release();
                activeFuses = -1;
            }

            else
            {
                foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
                {
                    if (fuseSlots.Contains(obj) && fuses[activeFuses].GetComponent<Fuse>().fuseSlot == obj)
                    {
                        fuses[activeFuses].GetComponent<Fuse>().Connect();
                        activeFuses = -1;

                        int connectedFuses = 0;
                        foreach (GameObject fuse in fuses)  // Check all fuses
                        {
                            if (fuse.GetComponent<Fuse>().connected) ++connectedFuses;  // If they are all connected
                        }

                        if (connectedFuses >= 4)
                        {
                            complete = true;
                            LightManager.Instance.AllLightToggle(true);
                            correctObject.GetComponent<Renderer>().material = correctMaterial;
                            incorrectObject.GetComponent<Renderer>().material = incorrectOffMaterial;
                            GetComponent<BoxCollider>().enabled = false;
                            PatientTaskManager.instance.CheckTaskConditions(gameObject);
                            StopUsing();
                        }

                        break;
                    }

                    else  // Not touching the fuse end
                    {
                        fuses[activeFuses].GetComponent<Fuse>().Release();
                    }
                }
            }
        }
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

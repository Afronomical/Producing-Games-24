using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class FuseBox : InspectableObject
{
    public enum Colours { Red, Blue, Yellow, Purple };

    [SerializeField] public GameObject cursor;
    [SerializeField] public List<GameObject> fuseSlots, fuses;
    [SerializeField] public Material[] materials;
    private int activeFuses;
    [SerializeField] public bool complete;



    public override void Interact()
    {
        base.Interact();
        GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        cursor.SetActive(true);
    }
    protected override void Start()
    {
        base.Start();
        InitializeBox();
        
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking) StopUsing();
    }


    public void InitializeBox()
    {
        complete = false;
        GetComponent<BoxCollider>().enabled = true;

        List<GameObject> w = new List<GameObject>(fuseSlots);
        for (int i = 3; i >= 0; i--)
        {
            int rand = Random.Range(0, w.Count);
            w[rand].GetComponent<Renderer>().material = materials[i];
            w.Remove(w[rand]);

        }

        foreach (GameObject obj in fuseSlots)
        {
            obj.GetComponent<Fuse>().InitializeFuse();
        }
    }

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
                    if (fuseSlots.Contains(obj) && fuseSlots[activeFuses].GetComponent<Fuse>().fuseSlot == obj)
                    {
                        fuses[activeFuses].GetComponent<Fuse>().Connect();
                        activeFuses = -1;

                        int connectedFuses = 0;
                        foreach (GameObject wire in fuseSlots)  // Check all fuses
                        {
                            if (wire.GetComponent<Fuse>().connected) ++connectedFuses;  // If they are all connected
                        }

                        if (connectedFuses >= 4)
                        {
                            complete = true;
                            //PatientTaskManager.instance.CheckTaskConditions(gameObject);
                            GetComponent<BoxCollider>().enabled = false;
                            StopUsing();
                        }

                        break;
                    }

                    else  // Not touching the wire end
                    {
                        fuseSlots[activeFuses].GetComponent<Fuse>().Release();
                    }
                }
            }
        }
    }
    private void StopUsing()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(false);

        looking = false;
        playerCanMove = true;
        stopLooking = true;
    }
}

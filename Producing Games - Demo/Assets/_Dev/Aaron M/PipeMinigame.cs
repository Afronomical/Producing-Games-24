using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMinigame : InspectableObject
{
    public PipeValve valve;
    public GameObject cursor;
    public List<GameObject> pipes;
    public List<Transform> removedPositions;
    [HideInInspector] public List<GameObject> removedPipes;
    public int partsToRemove;
    private int activePipe;
    public bool complete;



    public override void Interact()
    {
        base.Interact();

        GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        cursor.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking) StopUsing();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void InitializePuzzle()
    {
        valve.waterOn = true;
        valve.collectible = valve.turnOffSO;
        valve.GetComponent<BoxCollider>().enabled = true;
        valve.puzzle = this;

        complete = false;

        removedPipes.Clear();
        List<GameObject> w = new List<GameObject>(pipes);
        for (int i = 0; i < partsToRemove; i++)
        {
            if (w.Count > 0)
            {
                int rand = Random.Range(0, w.Count - 1);
                w[rand].transform.GetChild(0).GetComponent<PipePart>().RemovePart(removedPositions[i]);
                removedPipes.Add(w[rand].transform.GetChild(0).gameObject);
                w.Remove(w[rand]);
            }
        }
    }

    public void Select()
    {
        foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
        {
            if (removedPipes.Contains(obj) && !obj.GetComponent<PipePart>().connected)
            {
                activePipe = removedPipes.IndexOf(obj);
                obj.GetComponent<PipePart>().Hold();
                break;
            }
        }
    }

    public void Release()
    {
        if (activePipe != -1)
        {
            if (cursor.GetComponent<MinigameCursor>().collidingObjects.Count == 0)
            {
                removedPipes[activePipe].GetComponent<SatelliteWire>().Release();
                activePipe = -1;
            }

            else
            {
                foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
                {
                    if (pipes.Contains(obj) && removedPipes[activePipe].transform.parent.gameObject == obj)
                    {
                        removedPipes[activePipe].GetComponent<PipePart>().Connect();
                        activePipe = -1;

                        int connectedPipes = 0;
                        foreach (GameObject part in removedPipes)  // Check all pipes
                        {
                            if (part.GetComponent<PipePart>().connected) ++connectedPipes;  // If they are all connected
                        }

                        if (connectedPipes >= partsToRemove)
                        {
                            complete = true;
                            GetComponent<BoxCollider>().enabled = false;
                            valve.GetComponent<BoxCollider>().enabled = true;
                            removedPipes.Clear();
                            StopUsing();
                        }

                        break;
                    }

                    else  // Not touching the wire end
                    {
                        removedPipes[activePipe].GetComponent<PipePart>().Release();
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
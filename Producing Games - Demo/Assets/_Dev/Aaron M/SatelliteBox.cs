using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SatelliteBox : InspectableObject
{
    public enum Colours { Red, Blue, Yellow, Purple};

    public GameObject cursor;
    public List<GameObject> wires, wireEnds;
    public Material[] materials;
    private int activeWire;
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
        InitializeBox();
        base.Start();
    }

    public void InitializeBox()
    {
        complete = false;
        GetComponent<BoxCollider>().enabled = true;

        List<GameObject> w = new List<GameObject>(wires);
        List<GameObject> e = new List<GameObject>(wireEnds);
        for (int i = 3; i >= 0; i--)
        {
            int rand = Random.Range(0, w.Count);
            SatelliteWire currentWire = w[rand].GetComponent<SatelliteWire>();
            currentWire.colour = (Colours)i;
            w[rand].GetComponent<Renderer>().material = materials[i];
            currentWire.lineRenderer.material = materials[i];
            w.Remove(w[rand]);

            rand = Random.Range(0, e.Count);
            e[rand].GetComponent<Renderer>().material = materials[i];
            currentWire.wireEnd = e[rand];
            e.Remove(e[rand]);
        }

        foreach (GameObject obj in wires)
        {
            obj.GetComponent<SatelliteWire>().InitializeWire();
        }
    }

    public void Select()
    {
        foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
        {
            Debug.Log(obj.name);
            Debug.Log(obj.GetComponent<SatelliteWire>().connected);
            if (wires.Contains(obj) && !obj.GetComponent<SatelliteWire>().connected)
            {
                Debug.Log(obj);
                activeWire = wires.IndexOf(obj);
                obj.GetComponent<SatelliteWire>().Hold();
                break;
            }
        }
    }

    public void Release()
    {
        if (activeWire != -1)
        {
            if (cursor.GetComponent<MinigameCursor>().collidingObjects.Count == 0)
            {
                wires[activeWire].GetComponent<SatelliteWire>().Release();
                activeWire = -1;
            }

            else
            {
                foreach (GameObject obj in cursor.GetComponent<MinigameCursor>().collidingObjects)
                {
                    if (wireEnds.Contains(obj) && wires[activeWire].GetComponent<SatelliteWire>().wireEnd == obj)
                    {
                        wires[activeWire].GetComponent<SatelliteWire>().Connect();
                        activeWire = -1;

                        int connectedWires = 0;
                        foreach (GameObject wire in wires)  // Check all wires
                        {
                            if (wire.GetComponent<SatelliteWire>().connected) ++connectedWires;  // If they are all connected
                        }

                        if (connectedWires >= 4)
                        {
                            complete = true;
                            PatientTaskManager.instance.CheckTaskConditions(gameObject);
                            GetComponent<BoxCollider>().enabled = false;
                            StopUsing();
                        }

                        break;
                    }

                    else  // Not touching the wire end
                    {
                        wires[activeWire].GetComponent<SatelliteWire>().Release();
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

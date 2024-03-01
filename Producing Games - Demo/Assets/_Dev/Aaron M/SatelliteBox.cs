using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SatelliteBox : InspectableObject
{
    public enum Colours { Red, Blue, Yellow, Purple};

    public GameObject virtualMouse;
    public List<GameObject> wires, wireEnds;
    private int activeWire;

    public override void Interact()
    {
        base.Interact();

        GetComponent<BoxCollider>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        virtualMouse.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking) StopUsing();
    }

    public void InitializeBox()
    {

    }

    public void Select()
    {
        foreach (GameObject obj in virtualMouse.GetComponent<VirtualMouse>().collidingObjects)
        {
            if (wires.Contains(obj) && !obj.GetComponent<SatelliteWire>().connected)
            {
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
            foreach (GameObject obj in virtualMouse.GetComponent<VirtualMouse>().collidingObjects)
            {
                if (wireEnds.Contains(obj) && wires[activeWire].GetComponent<SatelliteWire>().wireEnd == obj)
                {
                    wires[activeWire].GetComponent<SatelliteWire>().Connect();
                    activeWire = -1;

                    int connectedWires = 0;
                    foreach(GameObject wire in wires)  // Check all wires
                    {
                        if (wire.GetComponent<SatelliteWire>().connected) ++connectedWires;  // If they are all connected
                    }

                    if (connectedWires >= 4)
                    {
                        // End task

                        StopUsing();
                    }

                    break;
                }
            }

            if (activeWire == -1)
            {
                wires[activeWire].GetComponent<SatelliteWire>().Release();
                activeWire = -1;
            }
        }
    }

    private void StopUsing()
    {
        Cursor.lockState = CursorLockMode.Locked;
        virtualMouse.SetActive(false);
        GetComponent<BoxCollider>().enabled = true;
    }
}

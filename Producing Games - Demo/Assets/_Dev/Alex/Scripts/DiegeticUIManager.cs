using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiegeticUIManager : MonoBehaviour
{
    public static DiegeticUIManager Instance;

    //Used to block player from using the documents before picking them up
    public bool hasChecklist = false;
    public bool hasDemonBook = false;
    public bool hasPager = false;
    public bool pagerBroken = false;

    //Used to toggle the cardiographs
    public List<HRM> hRMs = new List<HRM>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Takes the specific monitor that needs to be on as a parameter - 
    /// Monitor can be grabbed from the same instance through hRMs[indexofwantedmonitor]
    /// where the index is a number from 0 to 3
    /// </summary>
    /// <param name="monitor"></param>
    public void TurnOn(HRM monitor)
    {
        monitor.TurnOn();
    }

    /// <summary>
    /// Takes the specific monitor that needs to be off as a parameter - 
    /// Monitor can be grabbed from the same instance through hRMs[indexofwantedmonitor]
    /// where the index is a number from 0 to 3
    /// </summary>
    /// <param name="monitor"></param>
    public void TurnOff(HRM monitor)
    {
        monitor.TurnOff();
    }
}

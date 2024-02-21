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

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}

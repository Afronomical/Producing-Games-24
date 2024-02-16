using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityEventTracker : MonoBehaviour
{
    public enum SanityLevels { Sane, Delirious, Derranged, Hysterical, Madness};

    public int newHourSanity;
    public int hourlyTaskComplete;
    public int allHourlyTasksComplete;
    public int resetDefamedCross;
    public int defamedCrossNotReset;
    public int returnNPCToBed;
    public int NPCNotReturnedToBed;
    public int leaveNPCDuringEscort;
    public int NPCPraying;

    [Space(10)]
    public int reductionOverTime;
    public float timeBetweenReductionOverTime;
    private float reductionOverTimeTimer;

    private GameManager gm;
    private PatientTaskManager tm;


    void Start()
    {
        gm = GameManager.Instance;
        tm = PatientTaskManager.instance;
    }


    public void ChangeSanity(int sanity)  // Called when the gamemanager changes the sanity, this will set the sanity level enum
    {
        if (sanity >= 80)
            gm.sanityLevel = SanityLevels.Sane;
        else if (sanity >= 60)
            gm.sanityLevel = SanityLevels.Delirious;
        else if (sanity >= 40)
            gm.sanityLevel = SanityLevels.Derranged;
        else if (sanity >= 20)
            gm.sanityLevel = SanityLevels.Hysterical;
        else
            gm.sanityLevel = SanityLevels.Madness;
    }


    public void CompleteHourlyTask()
    {
        // Task complete
        gm.AddSanity(hourlyTaskComplete);

        // All tasks complete
        bool hasAnHourlyTask = false;
        for (int i = tm.currentTasks.Count - 1; i >= 0; i--)
        {
            if (tm.currentTasks[i].isHourlyTask)
                hasAnHourlyTask = true;
        }
        if (!hasAnHourlyTask) gm.AddSanity(allHourlyTasksComplete);
    }

    public void EndHour()
    {
        // New hour
        gm.AddSanity(allHourlyTasksComplete);

        // NPCNotReturnedToBed;
        /*foreach (GameObject AI in NPCManager.Instance.NPCS)
        {
            if (AI.GetComponent<AICharacter>().)  <---- If is in bed
        }*/

        // defamedCrossNotReset;

    }

    // resetDefamedCross;
    // NPCPraying;
    // returnNPCToBed;
    // leaveNPCDuringEscort;
}

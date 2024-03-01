using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChanceCalculator : MonoBehaviour
{
    public SanityEventTracker.SanityLevels PlayerSanity;

    GameManager player;

    [HideInInspector] public int eventChance;

    private void Update()
    {
        ChanceCalculator();
    }

    void ChanceCalculator()
    {
        player.GetSanity();
        

        if (player.sanityLevel == SanityEventTracker.SanityLevels.Sane)
            eventChance = 10;
        else if (player.sanityLevel == SanityEventTracker.SanityLevels.Delirious)
            eventChance = 20;
        else if (player.sanityLevel == SanityEventTracker.SanityLevels.Derranged)
            eventChance = 30;
        else if (player.sanityLevel == SanityEventTracker.SanityLevels.Hysterical)
            eventChance = 35;
        else if (player.sanityLevel == SanityEventTracker.SanityLevels.Madness)
            eventChance = 40;
    }
    public int DynamicEventChance()
    {
        return eventChance;
    }
}

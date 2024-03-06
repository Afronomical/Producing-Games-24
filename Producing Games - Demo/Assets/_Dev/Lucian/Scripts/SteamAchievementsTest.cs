using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievementsTest : MonoBehaviour
{
    public void GetAchievementStatus(string id)
    {
        SteamUserStats.GetAchievement(id, out bool completed);
        Debug.Log($"Achievement {id} is " +  completed);
    }

    public void SetAchievementStatus(string id)
    {
        SteamUserStats.SetAchievement(id);
        Debug.Log($"Achievement {id} is set");
    }

    public void ClearAchievement(string id)
    {
        SteamUserStats.ClearAchievement(id);
        Debug.Log($"Achievement {id} is cleared");
    }
}

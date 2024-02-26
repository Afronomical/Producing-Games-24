using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Steamworks;
using UnityEngine.SocialPlatforms.Impl;

public class ConsumeItem : MonoBehaviour
{
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;


    public void OnConsumeItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //steam achievement for consuming first pill
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetAchievement("NEW_ACHIEVEMENT_0_4", out bool completed);

                if (!completed)
                {
                    m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
                    
                    SteamUserStats.SetAchievement("NEW_ACHIEVEMENT_0_4");
                    SteamUserStats.StoreStats();
                }
            }

            if (PlayerInteractor.instance.consumable != null)
            {

                PlayerInteractor.instance.consumable.Consume();
                PlayerInteractor.instance.consumable = null;
                InventoryHotbar.instance.ScrollInventory(1);
            }

        }
    }


    void OnAchievementStored(UserAchievementStored_t pCallback)
    {

    }
}

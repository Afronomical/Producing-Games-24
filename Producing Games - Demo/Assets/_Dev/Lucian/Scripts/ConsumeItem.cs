using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumeItem : MonoBehaviour
{
    public void OnConsumeItem(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //steam achievement for consuming first pill
            //if(SteamManager.Initialized)
            //{
            //    Steamworks.SteamUserStats.GetAchievement("ConsumeFirstPill", out bool completed);

            //    if(!completed)
            //    {
            //        SteamUserStats.SetAchievement("ConsumeFirstPill");
            //        SteamUserStats.StoreStats();
            //    }
            //}

            if (PlayerInteractor.instance.consumable != null)
            {

                PlayerInteractor.instance.consumable.Consume();

            }

        }
    }
}

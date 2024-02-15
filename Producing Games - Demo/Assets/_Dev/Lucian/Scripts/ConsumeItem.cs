using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Steamworks;

public class ConsumeItem : MonoBehaviour
{
    public void OnConsumeItem(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //steam achievement for consuming first pill
            if (SteamManager.Initialized)
            {
                Steamworks.SteamUserStats.GetAchievement("ACH_WIN_ONE_GAME", out bool completed);

                if (!completed)
                {
                    SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
                    SteamUserStats.StoreStats();
                }
            }

            if (PlayerInteractor.instance.consumable != null)
            {

                PlayerInteractor.instance.consumable.Consume();

            }

        }
    }
}

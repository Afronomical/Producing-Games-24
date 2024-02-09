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
            if (PlayerInteractor.instance.consumable != null)
            {

                PlayerInteractor.instance.consumable.Consume();

            }

        }
    }
}

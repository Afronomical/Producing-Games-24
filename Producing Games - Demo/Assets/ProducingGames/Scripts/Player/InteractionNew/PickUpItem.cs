using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(PlayerInteractor.instance.currentObject != null) 
            {
                PlayerInteractor.instance.currentObject.Interact();
            
            }
        }
    }
}

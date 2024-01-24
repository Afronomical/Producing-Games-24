using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : Interactable
{
    public static event HandleItemCollected OnItemCollected;
    public delegate void HandleItemCollected(InteractiveObject itemData);

    public override void Interact()
    {
        //DialogueManager.manager.PlaySound(thisObject.sound);
        Destroy(gameObject);
        OnItemCollected?.Invoke(thisObject);
    }
}
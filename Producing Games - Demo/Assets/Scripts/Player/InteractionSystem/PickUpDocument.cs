using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDocument : Interactable
{
    public static event HandleItemCollected OnDocumentCollected;
    public delegate void HandleItemCollected(Document documentData, GameObject obj);

    public override void Interact()
    {
        //DialogueManager.manager.PlaySound(thisDocument.sound);
        OnDocumentCollected?.Invoke(thisDocument, gameObject);
    }
}

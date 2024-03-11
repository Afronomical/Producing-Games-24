using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteractible : InteractableTemplate
{
    public override void Interact()
    {
        DiegeticUIManager.Instance.hasDemonBook = true;
        Destroy(gameObject);
    }
}

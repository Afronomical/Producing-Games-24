using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipBoardInteractible : InteractableTemplate
{
    public override void Interact()
    {
        DiegeticUIManager.Instance.hasChecklist = true;
        Destroy(gameObject);
    }
}

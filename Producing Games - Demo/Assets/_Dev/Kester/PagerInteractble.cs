using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagerInteractble : InteractableTemplate
{

    
    public override void Interact()
    {
        DiegeticUIManager.Instance.hasPager = true;
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBaseClass : MonoBehaviour
{
    //public InteractableTemplate IObject;

    public string Name => throw new System.NotImplementedException();

    private void Start()
    {
        //IObject = GetComponent<InteractableTemplate>();
    }
    public virtual void Interact()
    {

    }

    public void Collect()
    {
        //throw new System.NotImplementedException();
    }
}

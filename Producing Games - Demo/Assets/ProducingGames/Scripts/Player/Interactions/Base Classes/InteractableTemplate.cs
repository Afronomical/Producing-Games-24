using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTemplate : MonoBehaviour, IInteractable
{
    //public UnityEvent onInteract;
    //public int ID;
    public Sprite objectIcon;
    //public Vector2 iconSize;
    [SerializeField] public InteractiveObject collectible;
    [SerializeField] public bool isExorcismObject;
    public bool canBeInteractedWith = true;

    public string Name => throw new System.NotImplementedException();

    public virtual void Collect()
    {
        
    }

    public virtual void Interact()
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTemplate : MonoBehaviour, IInteractable
{
    public UnityEvent onInteract;
    public int ID;
    public Sprite objectIcon;
    public Vector2 iconSize;
    [SerializeField] public InteractiveObject collectible;
    
    public enum ObjectType
    {
        Paper,
        Stick,
        Cube
    }

    [SerializeField] public ObjectType type;



    public void Collect()
    {
        Destroy(gameObject);
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + collectible.name);
        Collect();
    }

    public string Name { get; }
}

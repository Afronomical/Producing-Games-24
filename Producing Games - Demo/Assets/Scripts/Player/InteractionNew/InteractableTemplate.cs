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

    Vector3 doorPos;
    public enum ObjectType
    {
        Paper,
        Stick,
        Cube,
        Door
    }

    [SerializeField] public ObjectType type;



    public void Collect()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + collectible.name);
    }

    public void OpenDoor()
    {
        doorPos = transform.position;
        doorPos += new Vector3(0, 10, 0);
        gameObject.transform.position  = doorPos;

    }

    public string Name { get; }
}

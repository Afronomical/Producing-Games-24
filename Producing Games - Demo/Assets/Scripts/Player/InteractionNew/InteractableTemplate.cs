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

    private bool doorOpened = false;
    Vector3 endPosition;
    public enum ObjectType
    {
        Paper,
        Stick,
        Cube,
        Door,
        HolyWater,
        BlessedOil,
        FlashLight,
        Beads,
        Medicine,
        Syringe,
        Candles,
        Cabinet
    }

    [SerializeField] public ObjectType type;
    private void Start()
    {
        endPosition = transform.position + (Vector3.up * 10);
    }
    private void Update()
    {
        if (doorOpened)
        {
            doorPos = Vector3.Lerp(transform.position, endPosition , Time.deltaTime);
            gameObject.transform.position = doorPos;
        }
    }

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
        doorOpened = true;
       /* doorPos = transform.position;
        doorPos = Vector3.Lerp(transform.position, transform.position + (Vector3.up * 10), 1);//new Vector3(0, 10, 0);
        gameObject.transform.position  = doorPos;*/

    }

    public string Name { get; }
}

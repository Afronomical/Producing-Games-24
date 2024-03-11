using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{

    public FuseBox.Colours colour;//Colour reference
    private FuseBox box;//Fusebox script reference
    public bool followingMouse;
    public bool connected;
    public GameObject fuseSlot;
    public Transform returnPosition;//Reference for the fuse to be teleported back to if the slot is incorrect

    [HideInInspector] public List<Vector3> pointList = new List<Vector3>();

    //Handles if the fuse this is attached to is being held or if it's connected
    private void Update()
    {
        if (followingMouse == true)
            transform.position = box.cursor.transform.position; //Fuse will follow the minigame cursor

        if (connected == true)
            transform.position = fuseSlot.transform.position;//Fuse will connect to the correct slot
    }

    //When the fusebox is being setup and this gets called, it will put the fuse this is attached to, to the "return position"
    public void InitializeFuse()
    {
        box = GameObject.Find("FuseBox").GetComponent<FuseBox>();
        transform.position = returnPosition.position;
    }

    //Allows the fuse to be held (Handled in update)
    public void Hold() => followingMouse = true;
    
    //Stops the fuse following the cursor, if it's not the correct slot it will return back to the "return position"
    public void Release()
    {
        followingMouse = false;

        if(connected == false)
        {
            transform.position = returnPosition.position; 
        }
       
    }

    //Called and handled in FuseBox, this returns that this fuse is connected and the fuse will stop following the cursor
    public void Connect()
    {
        connected = true;
        followingMouse = false;
    }

}

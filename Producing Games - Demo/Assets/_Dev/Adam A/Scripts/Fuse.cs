using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public FuseBox.Colours colour;
    private FuseBox box;
    public bool followingMouse;
    public bool connected;
    public GameObject fuseSlot;
    public Transform returnPosition;

    [HideInInspector] public List<Vector3> pointList = new List<Vector3>();

    private void Update()
    {
        if (followingMouse)
        {
            gameObject.transform.position = box.cursor.transform.position;
        }

    }
    public void InitializeFuse()
    {
        box = transform.parent.GetComponent<FuseBox>();
    }


    public void Hold()
    {
        followingMouse = true;
    }


    public void Release()
    {
        followingMouse = false;

        gameObject.transform.position = returnPosition.position; 
        /*List<GameObject> correctSlot = new List<GameObject>(box.fuseSlots);
        foreach (GameObject slot in correctSlot) 
        {
            if(slot.GetComponent<Renderer>().material == gameObject.GetComponent<Renderer>().material)
            {
                gameObject.transform.position = slot.transform.position;
            }
        }*/
    }

    public void Connect()
    {
        connected = true;
        followingMouse = false;
    }

}

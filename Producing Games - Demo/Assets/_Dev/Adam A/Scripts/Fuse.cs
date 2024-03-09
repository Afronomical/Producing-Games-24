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

    private void Start()
    {
        Transform transform = gameObject.transform;
    }
    private void Update()
    {
        if (followingMouse == true)
        {
            transform.position = box.cursor.transform.position;
        }
        if(connected == true)
        {
            transform.position = fuseSlot.transform.position;
        }

    }
    public void InitializeFuse()
    {
        box = GameObject.Find("FuseBox").GetComponent<FuseBox>();
        transform.position = returnPosition.position;
    }


    public void Hold()
    {
        followingMouse = true;
    }


    public void Release()
    {
        followingMouse = false;

        if(connected == false)
        {
            transform.position = returnPosition.position; 
        }
       
    }

    public void Connect()
    {
        connected = true;
        followingMouse = false;
    }

}

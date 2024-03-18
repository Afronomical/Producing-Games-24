using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePart : MonoBehaviour
{
    private PipeMinigame puzzle;
    public bool followingMouse;
    public bool connected;
    public Transform pipeStart;

    public void RemovePart(Transform pos)
    {
        puzzle = transform.parent.parent.parent.GetComponent<PipeMinigame>();
        pipeStart = pos;
        transform.position = pipeStart.position;
        connected = false;
    }


    void Update()
    {
        if (followingMouse)
        {
            transform.position = puzzle.cursor.transform.position;
        }
    }


    public void Hold()
    {
        followingMouse = true;
        transform.parent.GetComponent<SphereCollider>().enabled = true;
    }


    public void Release()
    {
        followingMouse = false;
        transform.position = pipeStart.position;
        transform.parent.GetComponent<SphereCollider>().enabled = false;
    }


    public void Connect()
    {
        connected = true;
        transform.position = transform.parent.position;
        transform.parent.GetComponent<SphereCollider>().enabled = false;
        followingMouse = false;
    }
}

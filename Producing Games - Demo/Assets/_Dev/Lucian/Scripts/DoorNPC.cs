using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class DoorNPC : MonoBehaviour
{
    
    private NavMeshObstacle obstacle;

    public bool isOpen = false;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotAmount = 90f;
    [SerializeField]
    private float fwdDir = 0;

    public Transform doorHinge;

    private Vector3 startRotation;
    private Vector3 fwd;

    private Coroutine coroutine;

    private void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.carveOnlyStationary = false;
        obstacle.carving = isOpen;
        obstacle.enabled = isOpen;

        startRotation = doorHinge.transform.rotation.eulerAngles;
        fwd = transform.right;
    }

    public void OpenDoor(Vector3 agentPos)
    {
        if (!isOpen) 
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            float dot = Vector3.Dot(fwd, (agentPos - transform.position).normalized);
            coroutine = StartCoroutine(OpenDoorRotation(dot));
        }
    }

    private IEnumerator OpenDoorRotation(float fwd)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot;

        if(fwd > fwdDir)
        {
            endRot = Quaternion.Euler(new Vector3(startRot.x, startRot.y - rotAmount, startRot.z));
        }
        else
        {
            endRot = Quaternion.Euler(new Vector3(startRot.x, startRot.y + rotAmount, startRot.z));
        }

        isOpen = true;

        float time = 0;
        while(time < 1)
        {
            doorHinge.transform.rotation = Quaternion.Lerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        obstacle.enabled = true;
        obstacle.carving = true;
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(CloseDoorRotation());
        }
    }

    private IEnumerator CloseDoorRotation()
    {
        obstacle.enabled = false;
        obstacle.carving = false;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRotation);

        isOpen = false;

        float time = 0;
        while(time < 1)
        {
            doorHinge.transform.rotation = Quaternion.Slerp(startRot, endRot, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

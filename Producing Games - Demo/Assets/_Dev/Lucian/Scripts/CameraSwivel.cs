using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
    public Transform pivot;

    [Header("Camera Rotation Settings")]
    public bool canTurn;
    public float waitTime;
    public float spinTime;
    public float turnSpeed;

    private bool switchDirection;
    private float elapsedTime;
    private float internalTurnSpeed;

    private void Start()
    {
        switchDirection = false;
        elapsedTime = spinTime;
        internalTurnSpeed = turnSpeed;
    }
    private void FixedUpdate()
    {
        if(canTurn)
        {
            if(elapsedTime <= 0)
            {
                //SwitchDirection();
                internalTurnSpeed = 0;
                Invoke("SwitchDirection", waitTime);
                elapsedTime = spinTime + waitTime;
            }
            else
            {
                elapsedTime -= Time.deltaTime;
                transform.RotateAround(pivot.position, Vector3.up, switchDirection ? -internalTurnSpeed : internalTurnSpeed);
            }

        }

    }

    private void SwitchDirection()
    {
        
        switchDirection = !switchDirection;
        internalTurnSpeed = turnSpeed;
    }
}

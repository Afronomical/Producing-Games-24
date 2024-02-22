using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Bounce : MonoBehaviour
{
    [Header("Camera Reference")]
    public Camera cam;
    [Space]
    [Header("FOV Variables")]
    public float maxFOV; //FOV you want the cam to bounce between
    [Space]
    [Header("Bounce Variables")]
    public float changeSpeed; //Delta time will be multiplied by this to increase 'speed'
    public float bounceTime; //length of time bounce will last for, keep in mind that this also depends on speed variable (e.g faster changeSpeed, you'll reach bounce time sooner)



    bool isTriggered;
    private float initialFOV;

    void Start()
    {
        initialFOV = cam.fieldOfView;
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && isTriggered == false)
        {
            StartCoroutine(CamBounce());
            isTriggered = true;
        }
    }



    IEnumerator CamBounce()
    {
        float delta = 0f;

        while (delta < bounceTime)
        {
            delta += Time.deltaTime * changeSpeed;

            //Increase FOV til it reaches maxFOV then decrease
            switch (cam.fieldOfView < maxFOV)
            {
                case true:
                    cam.fieldOfView = cam.fieldOfView + delta;
                    break;
                case false:
                    cam.fieldOfView = cam.fieldOfView - delta;
                    break;
            }

            delta += Time.deltaTime;
            yield return null;
        }
        cam.fieldOfView = initialFOV;
        yield return new WaitForSeconds(1);
        

    }
}
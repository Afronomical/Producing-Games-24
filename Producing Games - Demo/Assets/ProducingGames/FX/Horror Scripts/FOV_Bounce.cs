using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para>The purpose of this script is to increase and decrease the FOV, once the player has entered the attached trigger box.</para> 
/// </summary>

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
    [Space]
    [Header("SFX")]
    public SoundEffect ScreenFXSound;

    private GameManager gM;
    bool isTriggered;
    private float initialFOV;

    void Start()
    {
        gM = GameManager.Instance;
        initialFOV = cam.fieldOfView;
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        int randChance = Random.Range(0, 101);

        if (other.CompareTag("Player") && randChance <= gM.eventChance)
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
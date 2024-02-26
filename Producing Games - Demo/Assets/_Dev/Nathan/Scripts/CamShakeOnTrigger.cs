using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para>The purpose of this script is to shake the player camera once the player has entered the attached trigger box.</para> 
/// </summary>

public class CamShakeOnTrigger : MonoBehaviour
{
    private bool triggered;
    [Header("Ref")]
    public CameraShake cameraShake;
    [Space]
    [Header("Shake Variables")]
    public float duration;
    public float intensity;
    [Space]
    [Header("SFX")]
    public SoundEffect CamShakeSound;

    private GameManager gM;
    private void OnTriggerEnter(Collider other)
    {
        int randChance = Random.Range(0, 101);

        if (other.CompareTag("Player") && randChance <= gM.eventChance)

        {
            StartCoroutine(cameraShake.CamShake(duration, intensity));
            triggered = true;
        }
        
    }
}

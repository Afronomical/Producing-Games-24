using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

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
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(cameraShake.CamShake(duration, intensity));
            triggered = true;
        }
        
    }
}

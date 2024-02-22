using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CamShakeOnTrigger : MonoBehaviour
{
    private bool triggered;
    public CameraShake cameraShake;

    public float duration;
    public float intensity;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            StartCoroutine(cameraShake.CamShake(duration, intensity));
            triggered = true;
        }
        
    }
}

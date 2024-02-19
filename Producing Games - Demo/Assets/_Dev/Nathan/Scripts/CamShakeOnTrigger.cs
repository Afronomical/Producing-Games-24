using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CamShakeOnTrigger : MonoBehaviour
{
    public CameraShake cameraShake;

    public float duration;
    public float intensity;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(cameraShake.CamShake(duration, intensity));
    }
}

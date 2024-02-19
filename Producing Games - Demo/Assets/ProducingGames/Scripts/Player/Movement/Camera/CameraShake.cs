using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


// To use this script, attach this script to the main camera
// The camera also needs to then be dragged into the 'camera shake' option in the inspector for the player movement script
// To call the camera shake in other scripts, make a reference of this script e.g "CameraShake camShake"
// Then call  StartCoroutine(camShake.CamShake(.15f, .2f)); NOTE first variable is duration, second is shake intensity

public class CameraShake : MonoBehaviour
{
    public IEnumerator CamShake (float duration, float magnitude) 
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration) 
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3 (x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}

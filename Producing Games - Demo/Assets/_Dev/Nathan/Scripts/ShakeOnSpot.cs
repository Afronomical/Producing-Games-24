using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para> A random item from the array will be chosen, this item will 'shake' until the time set has passed.</para> 
/// </summary>


public class ShakeOnSpot : MonoBehaviour
{
    bool isTriggered;

    [Header("Item List")]
    public Rigidbody[] Items;
    [Space]
    [Header("Shake Settings")]
    public float maxRotAngle = 45f;
    public float shakeTime;
    public float shakeForce;
    [Space]
    [Header("SFX")]
    public SoundEffect ItemShakeSound;

    private GameManager gM;
    void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int randChance = Random.Range(0, 101);

        if (other.CompareTag("Player") && randChance <= gM.eventChance)
        {
            StartCoroutine(ShakingOnSpot());
            isTriggered = true;
        }
    }

    IEnumerator ShakingOnSpot()
    {
        float startRotation = 0f;
        float delta = 0f;
        
        //chose a random item in list
        int randIndex = Random.Range(0, Items.Length);

        // Shake the item using max rotation angle and define time with shakeTime variable
        while (delta <= shakeTime)
        {
            switch (startRotation <= maxRotAngle)
            {
                case true:
                   
                    Items[randIndex].AddRelativeTorque(Vector3.forward * shakeForce, ForceMode.Impulse);
                    startRotation += Mathf.Abs(shakeForce);
                    break;
                case false:
                    Items[randIndex].AddRelativeTorque(Vector3.back * shakeForce, ForceMode.Impulse);
                    startRotation -= Mathf.Abs(shakeForce);
                    break;
            }
            delta += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
}

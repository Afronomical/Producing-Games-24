using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Floating_Items : MonoBehaviour
{
    bool isTriggered;

    [Header("Item List")]
    public Rigidbody[] Items;


    [Header("Ascension Forces")]
    public float minHeight;
    public float maxHeight;
    public float ascendSpeed;
    [Space]
    [Header ("Shake Forces")]
    public float shakeSpeed;
    public float shakeTime;   

    void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!isTriggered)
        {
            StartCoroutine(ShakingItem());
        }
    }

    IEnumerator ShakingItem()
    {
        float vertForce = Random.Range(minHeight, maxHeight);
        float horizForce = Random.Range(-5f, 5f);
        float ascendTime = 0f;
        
        ascendSpeed += (Time.deltaTime * ascendSpeed);

        int randIndex = Random.Range(0, Items.Length);

        Items[randIndex].useGravity = false;       

        while (ascendTime < 1f)
        {
            ascendTime += Time.deltaTime * ascendSpeed;
            Items[randIndex].transform.position = 
            new Vector3(Items[randIndex].position.x, (Mathf.Lerp(Items[randIndex].position.y, vertForce, ascendTime)), Items[randIndex].position.z);
            yield return null;
        }


        float startRotation = 0f;
        float targetRotation = 15f;


        while (startRotation != targetRotation)
        {
            switch (startRotation < targetRotation)
            {
                case true:
                    Items[randIndex].AddRelativeTorque(Vector3.right * horizForce * shakeSpeed, ForceMode.Impulse);
                    startRotation += Mathf.Abs(horizForce * shakeSpeed);
                    break;
                case false:
                    Items[randIndex].AddRelativeTorque(Vector3.left * horizForce * shakeSpeed, ForceMode.Impulse);
                    startRotation -= Mathf.Abs(horizForce * shakeSpeed);
                    break;
            }
        }
        yield return new WaitForSeconds(shakeTime);
        Items[randIndex].useGravity = true;
        isTriggered = true;
        /*// Rotate to the target angle
        if (startRotation < targetRotation)
        {
            Items[randIndex].AddRelativeTorque(Vector3.right * horizForce * shakeSpeed, ForceMode.Impulse);
            startRotation += Mathf.Abs(horizForce * shakeSpeed);
            yield return null;
        }

        // Reverse rotation
        if (startRotation >= targetRotation)
        {
            Items[randIndex].AddRelativeTorque(Vector3.left * horizForce * shakeSpeed, ForceMode.Impulse); 
            startRotation -= Mathf.Abs(horizForce * shakeSpeed);
            yield return null;
        }*/

        //Items[randIndex].AddRelativeTorque((Vector3.right * horizForce * shakeSpeed), ForceMode.Impulse);
        //yield return new WaitForSeconds(shakeTime);
    }
}

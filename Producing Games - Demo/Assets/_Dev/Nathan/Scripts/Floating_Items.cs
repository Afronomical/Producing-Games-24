using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

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
    [Header ("Shake Settings")]
    public float maxRotAngle = 45f;
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
            isTriggered = true;
        }

    }

    IEnumerator ShakingItem()
    {
        float vertForce = Random.Range(minHeight, maxHeight);
        float horizForce = Random.Range(-5f, 5f);
        float ascendTime = 0f;
        float startRotation = 0f; 
        float delta = 0f;

        ascendSpeed += (Time.deltaTime * ascendSpeed);

        //chose a random item in list
        int randIndex = Random.Range(0, Items.Length);

        Items[randIndex].useGravity = false;       

        // Ascend item, define possible heights and speeds in inspector
        while (ascendTime < 1f)
        {
            ascendTime += Time.deltaTime * ascendSpeed;
            Items[randIndex].transform.position = 
            new Vector3(Items[randIndex].position.x, (Mathf.Lerp(Items[randIndex].position.y, vertForce, ascendTime)), Items[randIndex].position.z);
            yield return null;
        }
         
        // Shake the item using max rotation angle and define time with shakeTime variable
        while (delta <= shakeTime)
        {
            switch (startRotation < maxRotAngle)
            {
                case true:
                    Items[randIndex].AddRelativeTorque(Vector3.right * horizForce, ForceMode.Impulse);
                    startRotation += Mathf.Abs(horizForce);
                    break;
                case false:
                    Items[randIndex].AddRelativeTorque(Vector3.left * horizForce, ForceMode.Impulse);
                    startRotation -= Mathf.Abs(horizForce);
                    break;
            }
            delta += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        Items[randIndex].useGravity = true;
        
    }
}

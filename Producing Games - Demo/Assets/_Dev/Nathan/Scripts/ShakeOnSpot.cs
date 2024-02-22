using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnSpot : MonoBehaviour
{
    bool isTriggered;

    [Header("Item List")]
    public Rigidbody[] Items;
    [Space]
    [Header("Shake Settings")]
    public float shakeForce;
    public float maxRotAngle = 45f;
    public float shakeTime;

    void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !isTriggered)
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

        Items[randIndex].useGravity = false;

        // Shake the item using max rotation angle and define time with shakeTime variable
        while (delta <= shakeTime)
        {
            switch (startRotation < maxRotAngle)
            {
                case true:
                    Items[randIndex].AddRelativeTorque(Vector3.up * shakeForce, ForceMode.Impulse);
                    startRotation += Mathf.Abs(shakeForce);
                    break;
                case false:
                    Items[randIndex].AddRelativeTorque(Vector3.up * shakeForce, ForceMode.Impulse);
                    startRotation -= Mathf.Abs(shakeForce);
                    break;
            }
            delta += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        Items[randIndex].useGravity = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownItems : MonoBehaviour
{
    bool isTriggered;

    [Header("Item List")]
    public Rigidbody[] Items;
    [Space]
    [Header("Random Forces")]
    public float minHorizontal;
    public float maxHorizontal;
    [Space]
    public float minVertical;
    public float maxVertical;
    
    void Start()
    {
        isTriggered = false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        //create random forces
        float vertForce = Random.Range(minVertical, maxVertical);
        float horizForce = Random.Range(minHorizontal, maxHorizontal);

        //adds relative force and torque to item (Check Z axis in scene view as this may change whether item flies forward or into wall)
        if (!isTriggered) 
        {
            int randIndex = Random.Range(0, Items.Length);
            Items[randIndex].AddRelativeForce((Vector3.up * vertForce) + (Vector3.forward * horizForce), ForceMode.Impulse);           
            Items[randIndex].AddRelativeTorque(Vector3.right, ForceMode.Impulse);
            isTriggered = true;        
        }
    }

}

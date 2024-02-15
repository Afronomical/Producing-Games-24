using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownItems : MonoBehaviour
{
    bool isTriggered;

    [Header("Item List")]
    public Rigidbody[] Items;
    

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
        float vertForce = Random.Range(minVertical, maxVertical);
        float horizForce = Random.Range(minHorizontal, maxHorizontal);

        if (!isTriggered) 
        {
            int randIndex = Random.Range(0, Items.Length);
            Items[randIndex].AddRelativeForce((Vector3.up * vertForce) + (Vector3.forward * horizForce), ForceMode.Impulse);           
            Items[randIndex].AddRelativeTorque(Vector3.right, ForceMode.Impulse);
            isTriggered = true;        
        }
    }

}

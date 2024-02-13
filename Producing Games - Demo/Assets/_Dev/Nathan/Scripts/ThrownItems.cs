using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownItems : MonoBehaviour
{
    bool isTriggered;
    public Rigidbody[] Items;
    public float verticalForce;
    public float horizontalForce;
    
    void Start()
    {
        isTriggered = false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered) 
        {
            int randIndex = Random.Range(0, Items.Length);
            Items[randIndex].AddRelativeForce((Vector3.up * verticalForce) + (Vector3.forward * horizontalForce), ForceMode.Impulse);
            isTriggered = true;        
        }
    }

}

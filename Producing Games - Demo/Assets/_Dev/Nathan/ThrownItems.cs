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
            int chance = UnityEngine.Random.Range(0, 2);
            if (chance == 0 ) 
            {
                Items[randIndex].AddForce(Vector3.up * verticalForce, ForceMode.Impulse);
                isTriggered = true;
            }
            if (chance == 1 ) 
            {
                Items[randIndex].AddForce(Vector3.forward * horizontalForce, ForceMode.Impulse);
                isTriggered = true;
            }

        }
    }

}

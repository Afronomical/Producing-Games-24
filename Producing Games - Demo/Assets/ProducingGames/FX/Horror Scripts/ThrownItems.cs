using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para> A random item will be selected from a list, this item will be 'thrown' using forces set in the inspector, 
/// this will happen once the player has entered the attached trigger box.</para> 
/// </summary>

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
    [Space]
    [Header("SFX")]
    public SoundEffect ThrowSound;
    private GameManager gM;


    void Start()
    {
        gM = GameManager.Instance;
        isTriggered = false; 
        
    }

    private void OnTriggerEnter(Collider other)
    {    
        
        //create random forces
        float vertForce = Random.Range(minVertical, maxVertical);
        float horizForce = Random.Range(minHorizontal, maxHorizontal);


        //adds relative force and torque to item (Check Z axis in scene view as this may change whether item flies forward or into wall)
        int randChance = Random.Range(0, 101);

        if (other.CompareTag("Player") && randChance <= gM.eventChance)
        {
            int randIndex = Random.Range(0, Items.Length);
            Items[randIndex].AddRelativeForce((Vector3.up * vertForce) + (Vector3.forward * horizForce), ForceMode.Impulse);           
            Items[randIndex].AddRelativeTorque(Vector3.right, ForceMode.Impulse);
            isTriggered = true;        
        }
    }

}

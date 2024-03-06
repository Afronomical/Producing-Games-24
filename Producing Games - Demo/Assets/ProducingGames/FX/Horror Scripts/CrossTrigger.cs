using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrossTrigger : MonoBehaviour
{
    public CrossBehaviour cross;
    GameManager gM;  

    private void Start()
    { 
        cross.eventTriggered = false;
        gM = GameManager.Instance;        
    }

    private void OnTriggerEnter(Collider other)
    {
        int randChance = Random.Range(0, 101);
        if (other.CompareTag("Player") && randChance <= gM.eventChance && !cross.eventTriggered)
        {
            cross.TriggerEvent();
            cross.eventTriggered = true;            
        }
    }

}

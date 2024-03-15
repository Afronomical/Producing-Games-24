using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossThrowTrigger : MonoBehaviour
{
    public CrossThrown cross;
    GameManager gM;
    [HideInInspector] public bool eventTriggered;
    
    
    private void Start()
    {
        eventTriggered = false;
        gM = GameManager.Instance;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int randChance = Random.Range(0, 101);
        if (other.CompareTag("Player") && randChance <= gM.eventChance && !eventTriggered)
        {
            cross.FallingCross();
            eventTriggered = true;
        }
    }

}

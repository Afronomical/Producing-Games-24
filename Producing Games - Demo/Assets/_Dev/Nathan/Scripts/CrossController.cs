using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    public CrossThrown crossThrow;
    public CrossRotation crossRotation;
    GameManager gM;
    [HideInInspector] public bool eventTriggered;
    private void Start()
    {
        //crossThrow.eventTriggered = false;
        gM = GameManager.Instance;
        eventTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int randChance = Random.Range(0, 101);
        if (other.CompareTag("Player") && randChance <= gM.eventChance && !eventTriggered)
        {
           TriggerEvent();
           
        }
    }

    public void TriggerEvent()
    {
        /*eventType = Random.Range(0, 2);
        if (eventType == 0)
        {
            crossRotation.isInverting = true;
            eventTriggered = true;
        }
        else 
        {
            crossThrow.FallingCross();
            eventTriggered = true;
        }*/
        if (Random.value < 0.5f)
        {
            crossThrow.FallingCross();
            eventTriggered = true;
        }    
        else
        {
            crossRotation.isInverting = true;
            eventTriggered = true;
        }
    }

}

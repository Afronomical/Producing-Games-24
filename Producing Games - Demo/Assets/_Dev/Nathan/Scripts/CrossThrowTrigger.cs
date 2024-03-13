using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossThrowTrigger : MonoBehaviour
{
    public CrossThrown crossThrow;

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
            crossThrow.FallingCross();
            eventTriggered = true;
        }
    }

}

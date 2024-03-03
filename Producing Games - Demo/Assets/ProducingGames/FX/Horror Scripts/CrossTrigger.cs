using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public class CrossTrigger : MonoBehaviour
{
    public CrossBehaviour cross;
    GameManager gM;  

    private void Start()
    {
        //transform.parent.GetComponent<CrossBehaviour>();
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
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerboxevents : MonoBehaviour
{
    public UnityEvent TutorialEvent;
    public GameObject tutorialManager;
    public Tutorialmanager tutmanagerScript;

    public bool inTrigger = false;

    public GameObject triggerbox;
    public bool startshift, deactivate, demonbook, demonrage, nxtshift, clickRequired, hour1objective;
    public static bool startshiftreq, demonbookreq, demonragereq, nextshiftreq;
    // public GameManager manager;
    // public GameObject gameManager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (clickRequired == false) { 
            if (demonbookreq && demonbook)
            {
                TutorialEvent.Invoke();
                deactivate = true;
            }
            if (demonrage && demonragereq)
            {
                TutorialEvent.Invoke();
                deactivate = true;
            }
            if (nxtshift && nextshiftreq)
            {
                TutorialEvent.Invoke();
                deactivate = true;
            }
            if (startshift && startshiftreq)
            {
                TutorialEvent.Invoke();
                deactivate = true;
            }
            }
            inTrigger = true;

        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        other = null;
    }

    public void Start()
    {
        triggerbox = this.gameObject;
        tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager");
        tutmanagerScript = tutorialManager.GetComponent<Tutorialmanager>();
        // manager = gameManager.GetComponent<GameManager>();
    }
    public void Update()
    {
        if (deactivate)
        {
            triggerbox.SetActive(false);
        }
        if (tutmanagerScript.startshift)
        {
            startshiftreq = true;
        }
        if (tutmanagerScript.demonCanRage)
        {
            demonragereq = true;
        }
        if (tutmanagerScript.nexthour)
        {
            nextshiftreq = true;
        }
        if (tutmanagerScript.demonbook)
        {
            demonbookreq = true;
        }

        if (inTrigger && Input.GetKeyDown(KeyCode.Mouse0)&&clickRequired)
        {
            if (hour1objective)
            {

           
            TutorialEvent.Invoke();
            deactivate = true;
            inTrigger = false; 
                clickRequired = false;
            }
        }

        if (inTrigger && Input.GetKeyDown(KeyCode.Mouse0) && nextshiftreq &&clickRequired)
        {
            TutorialEvent.Invoke();
          
            inTrigger = false;
        }
    }
}
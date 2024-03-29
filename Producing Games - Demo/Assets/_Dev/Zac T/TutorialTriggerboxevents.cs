using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerboxevents : MonoBehaviour
{
    public UnityEvent TutorialEvent;
    public Tutorialmanager tutorialManager;

   // public GameManager manager;
   // public GameObject gameManager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialEvent.Invoke();
        }
    }

    public void Start()
    {
       // manager = gameManager.GetComponent<GameManager>();
    }
    public void Update()
    {
       // if (tutorialManager.Task1 == true && manager.currentTime > 0)
       // {
        //    tutorialManager.OnShiftStartTask();
       // }
        
    }
}

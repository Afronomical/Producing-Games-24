using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerboxevents : MonoBehaviour
{
    public UnityEvent TutorialEvent;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialEvent.Invoke();
        }
    }
}

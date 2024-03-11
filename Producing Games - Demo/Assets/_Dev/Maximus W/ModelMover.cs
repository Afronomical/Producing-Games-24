using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class ModelMover : MonoBehaviour
{
    public Transform objectToMove;
    public Vector3 direction;
    public float distance = 1f;

    public SoundEffect Soundeffect;

    void Start()
    {
        
    }

    public void MoveObject()
    {
        if (objectToMove != null)
        {
            objectToMove.Translate(direction.normalized * distance);
            
            AudioManager.instance.PlaySound(Soundeffect, objectToMove.transform);
            
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned.");
        }
    }
}

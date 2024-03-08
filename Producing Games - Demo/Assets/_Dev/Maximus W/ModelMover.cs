using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMover : MonoBehaviour
{
    public Transform objectToMove;
    public Vector3 direction;
    public float distance = 1f;
    public AudioClip audioClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MoveObject()
    {
        if (objectToMove != null)
        {
            objectToMove.Translate(direction.normalized * distance);
            if (audioClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned.");
        }
    }
}

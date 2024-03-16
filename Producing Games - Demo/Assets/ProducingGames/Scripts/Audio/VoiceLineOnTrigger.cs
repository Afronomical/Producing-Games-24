using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineOnTrigger : MonoBehaviour
{
    [Header("Voice Line")]
    public SoundEffect[] VoiceLines;
    public GameObject VoicePosition;
    private bool hasPlayed;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayed = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !hasPlayed)
        {
            SoundEffect voiceLine = VoiceLines[Random.Range(0, VoiceLines.Length)];
            AudioManager.instance.PlaySound(voiceLine, gameObject.transform);
            hasPlayed = true;
        }
    }
}

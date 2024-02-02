using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenScript : MonoBehaviour
{
    public bool canSoundBeHeard = false;
    private void Start()
    {
        AudioListener listener = GetComponent<AudioListener>();
    }
    void Update()
    {
        if (canSoundBeHeard)
        {
            Debug.Log("Sound can be heard");
            canSoundBeHeard = false;
        }
    }
}

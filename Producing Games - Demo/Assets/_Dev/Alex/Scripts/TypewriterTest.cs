using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypewriterTest : MonoBehaviour
{
    public SoundEffect vA;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        DialogueTypewriter.instance.Dialogue(vA);
    }
}

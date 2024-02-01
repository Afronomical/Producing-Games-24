using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public AudioClip audioClip;

    [Range(0, 1)] public float volume = 0.75f;  // The volume of the sound (Will be affected by distance if it is spacial audio)
    [Range(-3, 3)] public float pitch = 1;  // The pitch of the sound
    [Range(0, 256)] public int priority = 128;  // The lower the priority, the more likely it is to be stolen by a sound with higher priority

    public bool loop = false;  // Whether the sound effect loops or not
    public bool isMusic = false;  // Whether the sound is music
    public bool canEcho = false;  // Whether sounds effects can echo  

    [Header("Spacial Audio")]
    public bool spacialAudio = true;  // Whether the sound effect is in the world or not (3D Audio)
    public bool followObject = false;  // Should the sound just play where it is called or should it move with the object
    [Range(0, 150)] public float travelDistance = 30;  // The max distance that you can hear the sound from

    [Header("Subtitles")]
    public string sentence = "Enter text here";  // The text that will be typed out
    public TMP_FontAsset font;  // The font that will be used for the text
    [Range(0, 0.25f)] public float typingSpeed = 0.15f;  // Time between each character appearing
    [Range(0, 10)] public float waitTillHide = 4.0f;  // Time till text dissapears after text has finished typing
}

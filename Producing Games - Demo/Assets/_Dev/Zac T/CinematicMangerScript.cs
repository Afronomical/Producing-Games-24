using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicMangerScript : MonoBehaviour
{
    [Header("Cinematic Manager Script")]
   

    [Header("Current Cinematic")]


    public PlayableDirector currentcinematic;


    public void Awake()
    {
        currentcinematic = GetComponent<PlayableDirector>(); 
    }
    public void Start()
    {

        StartJumpscare();
    }
    public void StartJumpscare()
    {
        print("play Jumpscare cutscene");
        currentcinematic.Play();
    }
}

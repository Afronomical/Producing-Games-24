using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CinematicMangerScript : MonoBehaviour
{
    [Header("Cinematic Manager Script")]


    [Header("Current Cinematic")]
    private float CinematicDuration=4f;
    private float CinematicTime;
    private bool cineStart;

    private GameObject playerOb;
    private PlayerInput playerin;
    private CharacterController playerCont;


    public PlayableDirector currentcinematic;


    public void Awake()
    {
        currentcinematic = GetComponent<PlayableDirector>();
        CinematicTime = CinematicDuration;
    }
    public void Start()
    {
        playerOb = GameManager.Instance.player;
        playerin=playerOb.GetComponent<PlayerInput>();
        playerCont = playerOb.GetComponent<CharacterController>();
        
    }
    public void FixedUpdate()
    {
        if (cineStart)
        {
            CinematicTime -= Time.deltaTime;
            print(CinematicTime);
            playerin.enabled = false;
            playerCont.enabled = false;
        }
        
        if (CinematicTime <= 0)
        {
            print("start end hour");
            StartCoroutine(GameManager.Instance.EndHour());
            cineStart = false;
           
            CinematicTime = CinematicDuration;
        }
    }   
    public void StartJumpscare()
    {
        CinematicTime = CinematicDuration;
        print("play Jumpscare cutscene");
        currentcinematic.Play();
       cineStart=true;
       
    }
}

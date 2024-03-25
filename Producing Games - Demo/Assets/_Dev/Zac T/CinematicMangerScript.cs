using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CinematicMangerScript : MonoBehaviour
{
    [Header("Cinematic Manager Script")]


    [Header("Current Cinematic")]
    private float basementCinematicDuration=6.48f;
    private float hallwayCinematicDuration = 4f;
    private float failedExorcismCinematicDuration = 4f;
    private float exorcismWinCinematicDuration = 18.35f;
    private float CinematicTime;
    private bool cineStart;
    public bool ishallwayCinematic;
   public bool isbasementCinematic;

    private GameObject playerOb;
    private PlayerInput playerin;
    private CharacterController playerCont;


    public PlayableDirector currentcinematic;
    public PlayableDirector hallwayCinematic;
    public PlayableDirector basementCinematic;
    public PlayableDirector failedExorcismCinematic;
    public PlayableDirector exorcismWinCinematic;
    public GameObject flashlight;

    private bool exorcismStarted;
    private bool exorcismSuccess;
    private bool exorcismFailed;

    public void Awake()
    {
        currentcinematic = gameObject.GetComponent<PlayableDirector>();
         
        CinematicTime = hallwayCinematicDuration;
    }
    public void Start()
    {
        playerOb = GameManager.Instance.player;
        playerin=playerOb.GetComponent<PlayerInput>();
        playerCont = playerOb.GetComponent<CharacterController>();
        flashlight = playerOb.GetComponentInChildren<PlayerArms>().flashlight;
        
    }
    public void FixedUpdate()

    {
        
        if (hallwayCinematic)
        {

            currentcinematic = gameObject.GetComponent<PlayableDirector>();
        }
        else if (basementCinematic)
        {

            currentcinematic = gameObject.GetComponent<PlayableDirector>();
        }

        if (cineStart)
        {
            CinematicTime -= Time.deltaTime;
        
            playerin.enabled = false;
            playerCont.enabled = false;
            flashlight.SetActive(false);
        }
        
        if (CinematicTime <= 0)
        {
            print("start end hour");
            flashlight.SetActive(true);
            if (exorcismStarted)
            {
                if (exorcismFailed)
                {
                    GameManager.Instance.exorcismFailed = true;
                    GameManager.Instance.EndGame(false);
                }
                else if (exorcismSuccess)
                {
                    GameManager.Instance.EndGame(true);
                }
            }
            else
            {
            StartCoroutine(GameManager.Instance.EndHour());
            }
            
            cineStart = false;
            exorcismStarted = false;
            CinematicTime = hallwayCinematicDuration;
        }
    }   
    public void StartHallwayJumpscare()
    {
        CinematicTime = hallwayCinematicDuration;
        print("play Jumpscare cutscene");
      hallwayCinematic.Play();
       cineStart=true;
       
    }
    public void StartBasementJumpscare()
    {
        CinematicTime = basementCinematicDuration;
        print("play basement cutscene");
        basementCinematic.Play();
        cineStart = true;
        
    }
    public void StartFailedExorcism()
    {
        CinematicTime = failedExorcismCinematicDuration;
        print("play basement cutscene");
        failedExorcismCinematic.Play();
        cineStart = true;
        exorcismStarted = true;
    }
    public void StartExorcismWin()
    {
        CinematicTime = exorcismWinCinematicDuration;
        print("play basement cutscene");
        exorcismWinCinematic.Play();
        cineStart = true;
        exorcismStarted = true;
    }
}

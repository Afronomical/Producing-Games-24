/*
 *This script should be placed on the AI character
 *It is responsible for the character's main logic
 * 
 * Written by Aaron & Adam
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]


public class AICharacter : MonoBehaviour
{
   public Rigidbody rb;
    public enum CharacterTypes
    {
        Patient,
        Demon
    }

    public enum States
    {
        Idle,
        Moving,
        Escorted,
        Wandering,
        Abandoned,
        Possessed,
        Bed,
        Exorcised,//??

        None
    }


    [Header("Character Stats")]
    public CharacterTypes characterType;
    public int startingHealth = 3;
    public int health;
    public int hungerValue = 1;
    public float walkSpeed, runSpeed, crawlSpeed;
    public float turnSpeed;
    public float turnDistance;
    public float DetectionRadius;
    public float step;
    public float EscortSpeed;
    private float MinSanity = 0;
    private float MaxSanity = 100;
    private float CurrentSanity; 

    [Header("States")]
    public States currentState;
    public StateBaseClass stateScript;
    public GameObject player;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool knowsAboutPlayer;


    void Start()
    {
        walkSpeed /= 2;
        runSpeed /= 2;
        crawlSpeed /= 2;
        health = startingHealth;
        CurrentSanity = MaxSanity; 
        EscortSpeed = 0.05f;
        ChangeState(States.Abandoned);  // The character will start in the idle state
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        DetectionRadius = 5f;
    }


    void Update()
    {
        if (stateScript != null)
            stateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts
    }


    public StateBaseClass GetCurrentState()  // Tell the script that called it which state is currently active
    {
        return stateScript;
    }


    public void ChangeState(States newState)  // Will destroy the old state script and create a new one
    {
        
        if (currentState != newState || stateScript == null)
        {
            if (stateScript != null)
            {
                //destroy current script attached to AI character
                Destroy(stateScript);
            }

            //set the current state of AI character to the new state
            currentState = newState;

            switch (newState)
            {
                
                case States.Idle:
                    stateScript = transform.AddComponent<IdleState>();
                    break;
                case States.Moving:
                    stateScript = transform.AddComponent<MovementState>();
                    break;
                case States.Exorcised:
                    stateScript = transform.AddComponent<ExorcisedState>();
                    break;
                case States.Possessed:
                    stateScript = transform.AddComponent<PosessedState>();
                    break;
                case States.Escorted:
                    stateScript = transform.AddComponent<EscortedState>();
                    break;
                case States.Abandoned:
                    stateScript = transform.AddComponent<AbandonedState>();
                    break;
                case States.Bed:
                    stateScript = transform.AddComponent<BedState>();
                    break;

                case States.None:
                    stateScript = null;
                    break;
                default:
                    stateScript = transform.AddComponent<IdleState>();
                    break;
            }

            if (stateScript != null)
                stateScript.character = this;  // Set the reference that state scripts will use
        }
    }


    public Vector2 GetPosition()
    {
        return transform.position;
    }


    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }


    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}

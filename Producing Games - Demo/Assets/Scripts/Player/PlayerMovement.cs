using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Unity.VisualScripting.Member;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;

    Vector3 moveVec = Vector3.zero;

    private InputAction move;
    private InputAction crouch;

    public float timeBetweenFootSteps;
    private float footStepTime;
    public GameObject audioManager;
    private AudioManager audioManagerInstance;
    private bool movementSoundCheck = false;

    private void OnEnable()
    {
        //move = playerControls.Player.Move;
        //move.Enable();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        //move.Disable();
        playerControls.Disable();
    }
    private void Awake()
    {
        //playerControls = new PlayerControls();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        audioManagerInstance = audioManager.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVec = playerControls.ReadValue<Vector3>();

        if (Input.GetKey(KeyCode.W))
        {
           footStepTime -= Time.deltaTime;
        
            if(footStepTime <= 0 )
            {
                footStepTime = timeBetweenFootSteps;
                audioManagerInstance.PlaySound(audioManagerInstance.WalkingSound, gameObject.transform);
            }
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            footStepTime = timeBetweenFootSteps;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveVec.x * moveSpeed, moveVec.y, moveVec.z * moveSpeed);
    }
}

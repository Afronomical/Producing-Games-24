using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;

    Vector3 moveVec = Vector3.zero;

    private InputAction move;
    private InputAction crouch;

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

        if (Input.GetKeyDown(KeyCode.W))
        {
           audioManagerInstance.PlaySound(audioManagerInstance.WalkingSound, gameObject.transform);
            
            
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            audioManagerInstance.StopSound(audioManagerInstance.WalkingSound);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveVec.x * moveSpeed, moveVec.y, moveVec.z * moveSpeed);
    }
}

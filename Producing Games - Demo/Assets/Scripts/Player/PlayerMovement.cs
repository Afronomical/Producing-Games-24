using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Ground Movement")]
    [Range(1, 15)] public float walkSpeed = 5;
    [Range(1, 15)] public float sprintSpeed = 8;
    [Range(1, 15)] public float crouchSpeed = 3;

    [Header("Air Movement")]
    [Range(0.05f, 1.5f)] public float jumpHeight = 0.5f;
    [Range(-20f, -0.05f)] public float gravity;
    [Range(0.1f, 0.6f)] public float groundCheckDistance;
    public LayerMask groundLayer;
    private Transform groundCheck;

    [Header("Current State")]
    [HideInInspector] public bool isGrounded;
    private bool isSprinting, isCrouching;
    private float yVelocity;

    [Header("Inputs")]
    private bool jumpInput;
    private Vector2 currentInput;

    [Header("Footstep Sounds")]
    // Walking
    public float timeBetweenWalkingFootsteps;
    public SoundEffect walkingSound;
    [Space(10)] // Sprinting
    public float timeBetweenSprintingFootsteps;
    public SoundEffect sprintingSound;
    [Space(10)] // Crouching
    public float timeBetweenCrouchingFootsteps;
    public SoundEffect crouchingSound;
    private float footstepTimer;




    private void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("Ground Check");
    }



    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);  // Check for ground beneath player
       
        Vector3 move = Movement();

        if (isGrounded && yVelocity < 0) yVelocity = -2;
        Jump();
        yVelocity += gravity * Time.deltaTime;  // Add gravity to the vertical velocity


        controller.Move(new Vector3(move.x, yVelocity, move.z) * Time.deltaTime);  // Move the player


        transform.localScale = new Vector3(transform.localScale.x, isCrouching ? 0.5f : 1f, transform.localScale.z);
        //controller.height = isCrouching ? 1f : 2f;  // Make character shorter when crouching

        FootstepSounds();
    }



    private Vector3 Movement()
    {
        Vector3 move = transform.right * currentInput.x + transform.forward * currentInput.y;  // Get input direction
        Vector3.Normalize(move);

        if (isCrouching) move *= crouchSpeed;  // Crouch movement
        else if (isSprinting) move *= sprintSpeed;  // Sprint movement
        else move *= walkSpeed;  // Basic movement

        return move;
    }



    private void Jump()
    {
        if (jumpInput)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);  // Jump
            jumpInput = false;
        }
    }



    private void FootstepSounds()
    {
        if ((currentInput.x != 0 || currentInput.y != 0) && isGrounded)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                if (isCrouching)
                {
                    footstepTimer = timeBetweenCrouchingFootsteps;
                    AudioManager.instance.PlaySound(crouchingSound, gameObject.transform);
                }

                else if (isSprinting)
                {
                    footstepTimer = timeBetweenSprintingFootsteps;
                    AudioManager.instance.PlaySound(sprintingSound, gameObject.transform);
                }

                else
                {
                    footstepTimer = timeBetweenWalkingFootsteps;
                    AudioManager.instance.PlaySound(walkingSound, gameObject.transform);
                }
            }
        }

        else
        {
            if (isCrouching) footstepTimer = timeBetweenCrouchingFootsteps;

            else if (isSprinting) footstepTimer = timeBetweenSprintingFootsteps;

            else footstepTimer = timeBetweenWalkingFootsteps;
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        currentInput = context.ReadValue<Vector2>();
    }


    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
            jumpInput = true;
        else if (context.canceled)
            jumpInput = false;
    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (!isSprinting && !isCrouching && context.performed)
            isSprinting = true;
        else if (isSprinting && context.canceled)
            isSprinting = false;
    }

    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (!isCrouching && context.performed)
        {
            isCrouching = true;
            isSprinting = false;
        }
        else if (isCrouching && context.canceled)
            isCrouching = false;
    }
}


/*public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rb;

    [Header("Player Movement Stats")]
    public float speed = 5;
    public float sprintSpeed = 10;
    public float jumpForce = 200;
    private Vector3 move;

    private bool isGrounded;
    private bool isSprinting, isCrouching;

    [Header("Footstep Sounds")]
    public float timeBetweenFootsteps;
    private float footstepTimer;
    public SoundEffect walkingSound;
    public SoundEffect runningSound;
    public SoundEffect crouchingSound;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        inputManager.playerControls.Player.Jump.started += _ => Jump();
        inputManager.playerControls.Player.DropItem.started += _ => DropItem();
    }


    private void Update()
    {
        isCrouching = inputManager.playerControls.Player.Crouch.ReadValue<float>() == 0 ? false : true;
        isSprinting = inputManager.playerControls.Player.Sprint.ReadValue<float>() == 0 || isCrouching ? false : true;

        float forward = inputManager.playerControls.Player.MoveForward.ReadValue<float>();
        float right = inputManager.playerControls.Player.MoveRight.ReadValue<float>();

        move = transform.right * right + transform.forward * forward;

        move *= isSprinting ? sprintSpeed : speed; //check if sprint button is being pressed

        transform.localScale = new Vector3(1, isCrouching ? 0.5f : 1f, 1); //check if player is crouching

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);


        if (move.x != 0 || move.z != 0)
        {
           footstepTimer -= Time.deltaTime;
        
            if(footstepTimer <= 0 )
            {
                footstepTimer = timeBetweenFootsteps;

                if(isCrouching)
                    AudioManager.instance.PlaySound(crouchingSound, gameObject.transform);
                    
                
                else if (isSprinting)
                    AudioManager.instance.PlaySound(runningSound, gameObject.transform);
                   
                else 
                    AudioManager.instance.PlaySound(walkingSound, gameObject.transform);
                  
               
            }
        }

        else if(move.x == 0 && move.z == 0)
        {
            footstepTimer = timeBetweenFootsteps;
        }
    }


    void Jump()
    {
        if (isGrounded)
            rb.AddForce(Vector3.up * jumpForce);
    }

    void DropItem()
    {
        if(InventoryHotbar.instance.inventory.Count == 0)
        {
            InventoryHotbar.instance.currentItem = null;

        }
        if(InventoryHotbar.instance.currentItem != null)
        {
            GameObject go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab,
                                    new Vector3(transform.position.x * 1.2f, transform.position.y, transform.position.z), Quaternion.identity);
            

            InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
*/
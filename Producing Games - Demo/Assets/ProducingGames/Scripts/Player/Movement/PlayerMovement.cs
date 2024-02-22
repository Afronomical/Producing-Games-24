using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Ground Movement")]
    [Range(1, 15)] public float walkSpeed = 5;
    [Range(1, 15)] public float sprintSpeed = 8;
    [Range(1, 15)] public float crouchSpeed = 3;
    public float crouchHeight = -1f;
    public GameObject crouchObject;
    [HideInInspector] public float defaultWalkSpeed;
    [HideInInspector] public float defaultSprintSpeed;
    [HideInInspector] public float defaultCrouchSpeed;
    private float maxStamina;
    [Range(1, 100)]public float stamina = 50;
    [Range(1, 100)] public float staminaDrainSpeed = 25;
    [Range(1, 100)] public float staminaRegenSpeed = 25;
    [Range(1, 100)] public float staminaRequiredToSprint = 25;//The amount of stamina the player needs to Sprint again

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

    [Header("Consumable Values")]
    public bool boostedEffect = false;
    public bool slowedEffect = false;
    public bool stoppedEffect = false;
    public bool dimmedEffect = false;
    public float boostedEffectDuration;
    public float slowedEffectDuration;
    public float stoppedEffectDuration;
    public float dimmedEffectDuration;

    //effect timer values
    private float currentSlowedTime;
    private float currentStoppedTime;
    private float currentBoostedTime;
    private float currentDimmedTime;
    [HideInInspector] public Image panel;

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

    public CameraShake cameraShake;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("Ground Check");

        maxStamina = stamina;



        currentBoostedTime = boostedEffectDuration;
        currentDimmedTime = dimmedEffectDuration;
        currentSlowedTime = slowedEffectDuration;
        currentStoppedTime = stoppedEffectDuration;
        panel = GameObject.Find("CameraDimOverlay").GetComponent<Image>();
        defaultWalkSpeed = walkSpeed;
        defaultSprintSpeed = sprintSpeed;
        defaultCrouchSpeed = crouchSpeed;
    }



    private void Update()
    {

        CheckEffect();
        
        if(boostedEffect)
            StartCoroutine(cameraShake.CamShake(7, 0.2f));
        


        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);  // Check for ground beneath player
       
        Vector3 move = Movement();

        if (isGrounded && yVelocity < 0) yVelocity = -2;
        Jump();
        yVelocity += gravity * Time.deltaTime;  // Add gravity to the vertical velocity


        controller.Move(new Vector3(move.x, yVelocity, move.z) * Time.deltaTime);  // Move the player


        //transform.localScale = new Vector3(transform.localScale.x, isCrouching ? 0.5f : 1f, transform.localScale.z);
        if (isCrouching) crouchObject.transform.SetLocalPositionAndRotation(new Vector3(0, crouchHeight, 0), crouchObject.transform.localRotation);
        else crouchObject.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), crouchObject.transform.localRotation);


        FootstepSounds();
        if (Input.GetKeyDown(KeyCode.O))
            StartCoroutine(cameraShake.CamShake(0.15f, .2f));
        
        if (!isSprinting && stamina <= maxStamina)
            stamina += staminaRegenSpeed * Time.deltaTime;

        if(stamina <= 1)
        {
            isSprinting = false;
        }
    }



    private Vector3 Movement()
    {
        Vector3 move = transform.right * currentInput.x + transform.forward * currentInput.y;  // Get input direction
        Vector3.Normalize(move);

        if (isCrouching) move *= crouchSpeed;  // Crouch movement

        else if (isSprinting)// Sprint movement
        {
            move *= sprintSpeed;
            stamina -= staminaDrainSpeed * Time.deltaTime;
        }
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
        if (!isSprinting && !isCrouching && context.performed && stamina >= staminaRequiredToSprint)
            isSprinting = true;
            
       
            
        else if (isSprinting && context.canceled || stamina <= 1)
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


    public void CheckEffect()
    {
        if(boostedEffect)
        {
            if (currentBoostedTime <= 0)
            {
                boostedEffect = false;
                walkSpeed = defaultWalkSpeed;
                sprintSpeed = defaultSprintSpeed;
                crouchSpeed = defaultCrouchSpeed;
                currentBoostedTime = boostedEffectDuration;
            }
            else
            {
                currentBoostedTime -= Time.deltaTime;
            }

        }
        if(slowedEffect)
        {
            if (currentSlowedTime <= 0)
            {
                slowedEffect = false;
                walkSpeed = defaultWalkSpeed;
                sprintSpeed = defaultSprintSpeed;
                crouchSpeed = defaultCrouchSpeed;
                currentSlowedTime = slowedEffectDuration;
                GetComponent<Flashlight>().light.intensity /= 10;
            }
            else
            {
                currentSlowedTime -= Time.deltaTime;
            }
        }
        if(stoppedEffect)
        {
            if(currentStoppedTime <= 0)
            {
                stoppedEffect = false;
                walkSpeed = defaultWalkSpeed;
                sprintSpeed = defaultSprintSpeed;
                crouchSpeed = defaultCrouchSpeed;
                currentStoppedTime = stoppedEffectDuration;

                if (GameManager.Instance.demon.activeSelf)
                {
                    GameManager.Instance.demon.GetComponent<DemonCharacter>().agent.velocity *= 2;

                }
            }
            else 
            {
                currentStoppedTime -= Time.deltaTime;
            }
        }
        if(dimmedEffect)
        {
            if(currentDimmedTime <= 0)
            {
                dimmedEffect = false;
                panel.enabled = false;
                currentDimmedTime = dimmedEffectDuration;
            }
            else
            {
                currentDimmedTime -= Time.deltaTime;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class PlayerMovement : MonoBehaviour
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

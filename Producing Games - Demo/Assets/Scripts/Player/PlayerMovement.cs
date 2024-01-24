using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputManager inputManager;

    public Rigidbody rb;

    [Header("Player Movement Stats")]
    public float speed = 5;
    public float sprintSpeed = 10;
    public float jumpForce = 200;

    private bool isGrounded;
    private void Start()
    {
        inputManager.playerControls.Player.Jump.started += _ => Jump();
    }

    private void Update()
    {
        float forward = inputManager.playerControls.Player.MoveForward.ReadValue<float>();
        float right = inputManager.playerControls.Player.MoveRight.ReadValue<float>();

        Vector3 move = transform.right * right + transform.forward * forward;

        move *= inputManager.playerControls.Player.Sprint.ReadValue<float>() == 0 ? speed : sprintSpeed; //check if sprint button is being pressed

        transform.localScale = new Vector3(1, inputManager.playerControls.Player.Crouch.ReadValue<float>() == 0 ? 1f : 0.5f, 1); //check if player is crouching

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void Jump()
    {
        if (isGrounded)
            rb.AddForce(Vector3.up * jumpForce);
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

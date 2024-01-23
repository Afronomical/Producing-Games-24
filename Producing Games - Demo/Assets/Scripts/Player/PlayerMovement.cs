using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;

    Vector3 moveVec = Vector3.zero;

    private InputAction move;
    private InputAction crouch;


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
    }

    // Update is called once per frame
    void Update()
    {
        moveVec = playerControls.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveVec.x * moveSpeed, moveVec.y, moveVec.z * moveSpeed);
    }
}

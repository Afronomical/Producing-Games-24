using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Camera Properties")]
    [Range(0, 1)] public float mouseSensitivity = 0.5f;
    private Vector2 currentInput;
    [Range(-80, 0)] public float downClampAngle = -50;
    [Range(0, 80)] public float upClampAngle = 50;

    public Transform playerBody;
    private PlayerMovement playerMovement;
    private CharacterController playerController;

    [Header("Head Bobbing")]
    [SerializeField] public bool canHeadBob = true;
    [SerializeField] [Range(0.1f, 5f)] private float bobAmplitude = 0.5f;
    [SerializeField][Range(5f, 15f)] private float bobFrequency = 10f;
    public float bobResetSpeed = 1.0f;
    private float bobOffSpeed = 3f;
    private Vector3 camStartPos;


    private float xRot = 0f;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camStartPos = transform.localPosition;
        playerMovement = playerBody.GetComponent<PlayerMovement>();
        playerController = playerBody.GetComponent<CharacterController>();
    }



    void Update()
    {
        float mouseX = currentInput.x / 5 * mouseSensitivity;
        float mouseY = currentInput.y / 5 * mouseSensitivity;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, downClampAngle, upClampAngle);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);  // Rotate the camera
        playerBody.Rotate(Vector3.up * mouseX);  // Rotate the player left and right

        // Head Bob
        if (canHeadBob)
        {
            CheckMovement();
        }
    }



    public void OnLookInput(InputAction.CallbackContext context)
    {
        currentInput = context.ReadValue<Vector2>();
    }



    private void CheckMovement()
    {
        //float speed = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude;
        float speed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude; //* 100f;
        if (speed < bobOffSpeed || !playerMovement.isGrounded) BobReset();

        else transform.localPosition += FootStepMotion();
        bobFrequency = Mathf.Lerp(0, 20, (speed / 10));
    }



    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * bobFrequency) * bobAmplitude * Time.deltaTime;
        pos.x += Mathf.Cos(Time.time * bobFrequency / 2) * bobAmplitude * 2 * Time.deltaTime;
        return pos;
    }
    
    private void BobReset()
    {
        if (transform.localPosition != camStartPos)
            transform.localPosition = Vector3.Lerp(transform.localPosition, camStartPos, Time.deltaTime * bobResetSpeed);
    }
}

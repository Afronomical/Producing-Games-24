using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Camera Properties")]
    public InputManager inputManager;
    public float mouseSensitivity = 30f;
    public Transform playerBody;

    [Header("Head bobbing")]
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private float bobAmplitude = 0.02f;
    [SerializeField] private float bobFrequency = 1.0f;
    private float bobOffSpeed = 3f;
    private Vector3 camStartPos;

    private Rigidbody playerRb;

    private float xRot = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        camStartPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = inputManager.playerControls.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        float mouseY = inputManager.playerControls.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        if (canHeadBob)
        {
            CheckMovement();
        }
        
        
    }
    
    private void CheckMovement()
    {
        float speed = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z).magnitude;
        if (speed < bobOffSpeed)
        {
            BobReset();
        }
        else
        {
            transform.localPosition += FootStepMotion();
        }
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        pos.x += Mathf.Cos(Time.time * bobFrequency / 2) * bobAmplitude * 2;
        return pos;
    }
    
    private void BobReset()
    {
        if (transform.localPosition != camStartPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, camStartPos, Time.deltaTime);
        }
    }
}

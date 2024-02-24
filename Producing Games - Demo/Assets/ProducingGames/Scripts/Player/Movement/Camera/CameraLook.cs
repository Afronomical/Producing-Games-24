using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class CameraLook : MonoBehaviour
{
    private Camera m_Camera;

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

    private bool isLeaning;
    [HideInInspector] public float xRot = 0f;


    void Start()
    {
        m_Camera = GetComponent<Camera>();
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
        Leaning();


        GameObject[] allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in allNPCs)
        {
            Plane[] fov = GeometryUtility.CalculateFrustumPlanes(m_Camera);

            if (GeometryUtility.TestPlanesAABB(fov, npc.GetComponent<Collider>().bounds))
                Debug.Log(npc.name + " is in view");
            
            else
                Debug.Log(npc.name + " is not in view");
        }

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

    private void Leaning()
    {
       /* To not be included in the build 17/02/24
        if (Input.GetKey(KeyCode.Q))
        {
            isLeaning = true;
            transform.localPosition = new Vector3(-0.45f, camStartPos.y, camStartPos.z);
            transform.localRotation = Quaternion.Euler(xRot, 0f, 20f);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            isLeaning = true;
            transform.localPosition = new Vector3(0.45f, camStartPos.y, camStartPos.z);
            transform.localRotation = Quaternion.Euler(xRot, 0f, -20f);
        }
        else
        {
            isLeaning = false;
            transform.localPosition = camStartPos;
            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }*/
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
        if (transform.localPosition != camStartPos && isLeaning == false)
            transform.localPosition = Vector3.Lerp(transform.localPosition, camStartPos, Time.deltaTime * bobResetSpeed);
    }
}

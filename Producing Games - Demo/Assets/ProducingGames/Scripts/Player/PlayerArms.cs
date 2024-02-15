using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public Transform playerBody;
    private PlayerMovement playerMovement;
    private CharacterController playerController;

    [Header("Arm Bobbing")]
    [SerializeField][Range(0.1f, 5f)] private float bobAmplitude = 0.5f;
    [SerializeField][Range(5f, 15f)] private float bobFrequency = 10f;
    public float bobResetSpeed = 1.0f;
    private float bobOffSpeed = 3f;
    private Vector3 startPos;


    void Start()
    {
        startPos = transform.localPosition;
        playerMovement = playerBody.GetComponent<PlayerMovement>();
        playerController = playerBody.GetComponent<CharacterController>();
    }


    void Update()
    {
        ArmBobbing();
    }


    private void ArmBobbing()
    {
        float speed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude; //* 100f;
        if (speed < bobOffSpeed || !playerMovement.isGrounded) BobReset();

        else transform.localPosition += BobMotion();
        bobFrequency = Mathf.Lerp(0, 20, (speed / 10));
    }

    private Vector3 BobMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * bobFrequency) * bobAmplitude * Time.deltaTime;
        pos.x += Mathf.Cos(Time.time * bobFrequency / 2) * bobAmplitude * 2 * Time.deltaTime;
        return pos;
    }

    private void BobReset()
    {
        if (transform.localPosition != startPos)
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * bobResetSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Written By: Aaron Moreland
/// 
/// Controls and animates the arms on the Player
/// </summary>


public class PlayerArms : MonoBehaviour
{
    private enum leftArmStates { Idle, Pager};
    private enum rightArmStates { Idle, Object, Clipboard };

    public Transform playerBody;
    private PlayerMovement playerMovement;
    private CharacterController playerController;
    private PickUpItem pickUpItem;
    public Animator leftAnimator, rightAnimator;
    public GameObject heldItem;

    private leftArmStates leftArmState;
    private rightArmStates rightArmState;

    [Range(0.01f, 1f)] public float grabItemTime = 0.4f;

    private bool holdingClipboard;
    private bool holdingObject;
    private bool holdingPager;

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
        pickUpItem = playerBody.GetComponent<PickUpItem>();
    }


    void Update()
    {
        if (holdingClipboard) rightArmState = rightArmStates.Clipboard;

        else if (holdingObject) rightArmState = rightArmStates.Object;

        else rightArmState = rightArmStates.Idle;



        if (holdingPager) leftArmState = leftArmStates.Pager;

        else leftArmState = leftArmStates.Idle;


        ArmBobbing();
    }




    public IEnumerator GrabObject()
    {
        rightAnimator.SetTrigger("Grab");  // Play the animation
        heldItem.SetActive(false);  // Hide the item in your hand
        pickUpItem.canPickUp = false;  // Stop PickUpItem from calling the enum again

        yield return new WaitForSeconds(grabItemTime);  // Wait until the hand reaches the object

        heldItem.SetActive(true);  // Show the item in your hand
        PlayerInteractor.instance.currentObject.Interact();  // Pick up the object in front of you
        pickUpItem.canPickUp = true;
    }


    public void HoldPager()  // Pick up the pager
    {
        leftAnimator.SetBool("Pager", true);
    }

    public void DropPager()  // Put down the pager
    {
        leftAnimator.SetBool("Pager", false);
    }




    private void ArmBobbing()  // A more subtle version of the head bobbing to make the arms move more naturally
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

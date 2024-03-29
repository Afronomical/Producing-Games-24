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
    public enum leftArmStates { Flashlight, Pager, Clipboard, Book};
    public enum rightArmStates { Idle, Object, Clipboard, Injection };

    public Transform playerBody;
    private PlayerMovement playerMovement;
    private CharacterController playerController;
    public GameObject flashlight, pager, pagerScreen, syringe, book;
    public GameObject clipboard, clipboardFlashlight;
    private PickUpItem pickUpItem;
    public Animator leftAnimator, rightAnimator;
    public GameObject heldItem;

    [HideInInspector] public leftArmStates leftArmState;
    [HideInInspector] public rightArmStates rightArmState;

    [Range(0.01f, 1f)] public float grabItemTime = 0.4f;

    private bool holdingClipboard;
    private bool holdingBook;
    private bool holdingObject;
    [HideInInspector] public bool holdingPager;
    public GameObject PagerLight;
    public GameObject FPagerLight;
    private bool holdingSyringe;
    private string lastLeftAnimation, lastRightAnimation;

    [Header("Arm Bobbing")]
    [SerializeField][Range(0.01f, 5f)] private float bobAmplitude = 0.5f;
    [SerializeField][Range(0.5f, 15f)] private float bobFrequency = 10f;
    public float bobResetSpeed = 1.0f;
    private float bobOffSpeed = 3f;
    private Vector3 startPos;


    void Start()
    {
        startPos = transform.localPosition;
        playerMovement = playerBody.GetComponent<PlayerMovement>();
        playerController = playerBody.GetComponent<CharacterController>();
        pickUpItem = playerBody.GetComponent<PickUpItem>();
        book.SetActive(true);
    }


    void Update()
    {
        if (holdingClipboard)
        {
            rightArmState = rightArmStates.Clipboard;
        }

        else if (holdingObject)
        {
            rightArmState = rightArmStates.Object;
        }

        else
        {
            rightArmState = rightArmStates.Idle;
        }



        if (holdingClipboard)
        {
            leftArmState = leftArmStates.Clipboard;
        }

        else if (holdingPager)
        {
            leftArmState = leftArmStates.Pager;
        }

        else
        {
            leftArmState = leftArmStates.Flashlight;
        }


        if (leftAnimator.GetCurrentAnimatorClipInfoCount(0) != 0)  // When the animation on the left arm changes
        {
            if (leftAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != lastLeftAnimation && leftAnimator.GetCurrentAnimatorStateInfo(0).speed > 0)
            {
                SetLeftHeldObjects();
                lastLeftAnimation = leftAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }
        }

        if (rightAnimator.GetCurrentAnimatorClipInfoCount(0) != 0)  // When the animation on the left arm changes
        {
            if (rightAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != lastRightAnimation && rightAnimator.GetCurrentAnimatorStateInfo(0).speed > 0)
            {
                SetRightHeldObjects();
                lastRightAnimation = rightAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }
        }


        if (pickUpItem.currentLocker != null || pickUpItem.currentlyInspecting != null)
        {
            leftAnimator.SetBool("Interacting", true);
            rightAnimator.SetBool("Interacting", true);

            pager.GetComponent<SkinnedMeshRenderer>().enabled = false;
            pagerScreen.GetComponent<Canvas>().enabled = false;
            if (FPagerLight.activeSelf)
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
            flashlight.gameObject.SetActive(false);
            clipboard.SetActive(false);
            book.SetActive(false);

            heldItem.SetActive(false);
            syringe.SetActive(false);
            clipboardFlashlight.SetActive(false);

        }
        else if (pickUpItem.currentLocker == null && pickUpItem.currentlyInspecting == null)
        {
            leftAnimator.SetBool("Interacting", false);
            rightAnimator.SetBool("Interacting", false);
        }

        ArmBobbing();
    }



    private void SetLeftHeldObjects()  // Change the items visible in your hands
    {
        if (flashlight.activeSelf && playerBody.GetComponent<Flashlight>().intensityIndex != 0) 
            AudioManager.instance.PlaySound(playerBody.GetComponent<Flashlight>().toggleSound, null);

        AnimatorStateInfo info = leftAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("ClipboardUp") || info.IsName("ClipboardIdleAnim"))
        {
            pager.GetComponent<SkinnedMeshRenderer>().enabled = false;
            pagerScreen.GetComponent<Canvas>().enabled = false;
            if (PagerLight.activeSelf)
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
            flashlight.gameObject.SetActive(false);
            clipboard.gameObject.SetActive(true);
            book.SetActive(false);
            PlayerVoiceController.instance.PlayDialogue(PlayerVoiceController.instance.checklistDialogue);
        }
        else if (info.IsName("BookUpAnim") || info.IsName("BookIdleAnim"))
        {
            pager.GetComponent<SkinnedMeshRenderer>().enabled = false;
            pagerScreen.GetComponent<Canvas>().enabled = false;
            if (FPagerLight.activeSelf)
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
            flashlight.gameObject.SetActive(false);
            clipboard.SetActive(false);
            book.SetActive(true);
        }
        else if (info.IsName("PagerUp"))
        {
            pager.GetComponent<SkinnedMeshRenderer>().enabled = true;
            pagerScreen.GetComponent<Canvas>().enabled = true;
            if (FPagerLight.activeSelf)
            {
                PagerLight.SetActive(true);
                FPagerLight.SetActive(false);
            }
            flashlight.gameObject.SetActive(false);
            clipboard.SetActive(false);
            book.SetActive(false);
        }
        else if (info.IsName("FlashlightUp"))
        {
            pager.GetComponent<SkinnedMeshRenderer>().enabled = false;
            pagerScreen.GetComponent<Canvas>().enabled = false;
            flashlight.gameObject.SetActive(true);
            clipboard.SetActive(false);
            if (PagerLight.activeSelf)
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
            book.SetActive(false);
        }
        else if (info.IsName("Default"))
        {
            pager.GetComponent<SkinnedMeshRenderer>().enabled = false;
            pagerScreen.GetComponent<Canvas>().enabled = false;
            flashlight.gameObject.SetActive(false);
            clipboard.SetActive(false);
            book.SetActive(false);
            if (PagerLight.activeSelf)
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
        }


        if (flashlight.activeSelf && playerBody.GetComponent<Flashlight>().intensityIndex != 0)
            AudioManager.instance.PlaySound(playerBody.GetComponent<Flashlight>().toggleSound, null);
    }



    private void SetRightHeldObjects()  // Change the items visible in your hands
    {
        AnimatorStateInfo info = rightAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("ClipboardFlashlightUp"))
        {
            heldItem.SetActive(false);
            syringe.SetActive(false);
            clipboardFlashlight.SetActive(true);
        }
        else if (info.IsName("Grab"))
        {
            heldItem.SetActive(false);
            syringe.SetActive(false);
            clipboardFlashlight.SetActive(false);
        }
        else if (info.IsName("Injection"))
        {
            heldItem.SetActive(false);
            syringe.SetActive(true);
            clipboardFlashlight.SetActive(false);
        }
        else if (info.IsName("HoldingUp"))
        {
            //heldItem.SetActive(true);
            clipboardFlashlight.SetActive(false);
            syringe.SetActive(false);
        }
        else if(info.IsName("Default"))
        {
            heldItem.SetActive(false);
            clipboardFlashlight.SetActive(false);
            syringe.SetActive(false);
        }
    }



    public IEnumerator GrabObject(InteractiveObject obj)
    {
        if (!holdingClipboard)
        {
            rightAnimator.SetTrigger("Grab");  // Play the animation
            heldItem.SetActive(false);  // Hide the item in your hand
            pickUpItem.canPickUp = false;  // Stop PickUpItem from calling the enum again

            yield return new WaitForSeconds(grabItemTime);  // Wait until the hand reaches the object

            //heldItem.SetActive(true);  // Show the item in your hand
            holdingObject = true;
            if (obj.interactSound != null)
                AudioManager.instance.PlaySound(obj.interactSound, null);
            PlayerInteractor.instance.currentObject.Interact();  // Pick up the object in front of you
            pickUpItem.canPickUp = true;
        }
    }


    public void HoldPager()  // Pick up the pager
    {
        leftAnimator.SetBool("Pager", true);
        holdingPager = true;
    }

    public void DropPager()  // Put down the pager
    {
        leftAnimator.SetBool("Pager", false);
        holdingPager = false;
    }


    public void HoldClipboard()  // Pick up the clipboard
    {
        leftAnimator.SetBool("Checklist", true);
        rightAnimator.SetBool("Checklist", true);
        holdingClipboard = true;
    }

    public void DropClipboard()  // Put down the Clipboard
    {
        leftAnimator.SetBool("Checklist", false);
        rightAnimator.SetBool("Checklist", false);
        holdingClipboard = false;
    }

    public void HoldBook()  // Pick up the book
    {
        leftAnimator.SetBool("Book", true);
        rightAnimator.SetBool("Checklist", true);
        holdingBook = true;
    }

    public void DropBook()  // Put down the book
    {
        leftAnimator.SetBool("Book", false);
        rightAnimator.SetBool("Checklist", false);
        holdingBook = false;
    }

    public void GiveInjection()  // Pick up the clipboard
    {
        rightAnimator.SetTrigger("Injection");
        holdingSyringe = true;
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

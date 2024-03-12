using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para> This system will look to check the terrain by using a raycast, from this will then determine the correct audio clip for
/// the terrain the player is currently walking on.</para> 
/// </summary>

public class DynamicFootsteps : MonoBehaviour
{
    [Header("Walking Foot Step Sounds")]
    public float timeBetweenWalkingFootsteps;
    public SoundEffect StoneFloorWalking;
    public SoundEffect CryptWalking;
    public SoundEffect CarpetWalking;
    [Space(10)]
    [Header("Sprinting Foot Step Sounds")]
    public float timeBetweenSprintingFootsteps;
    public SoundEffect StoneFloorSprinting;
    public SoundEffect CryptSprinting;
    public SoundEffect CarpetSprinting;
    [Space(10)]
    [Header("Crouching Foot Step Sounds")]
    public float timeBetweenCrouchingFootsteps;
    public SoundEffect StoneFloorCrouching;
    public SoundEffect CryptCrouching;
    public SoundEffect CarpetCrouching;

    [Space(10)]
    [Header("RayCast Variables")]
    public Transform RayStart;
    public float range;
    public LayerMask GroundLayer;

    public PlayerMovement PlMove;
    RaycastHit FootStepRay;
    private float footstepTimer;

    void Update()
    {
        DynamicFootstepSounds();     
    }

    /// <summary>
    /// Note -Some of these variables will be changed and moved from being a reference from the player movement script to being private here
    /// This will happen once this script has been fully implemented.
    /// </summary>
    private void DynamicFootstepSounds()
    {
        if ((PlMove.currentInput.x != 0 || PlMove.currentInput.y != 0) && PlMove.isGrounded)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                if (PlMove.isCrouching)
                {
                    footstepTimer = timeBetweenCrouchingFootsteps;
                    //AudioManager.instance.PlaySound(crouchingSound, gameObject.transform);
                    ChangeCrouchingFootSteps();
                }
                else if (PlMove.isSprinting)
                {
                    footstepTimer = timeBetweenSprintingFootsteps;
                    //AudioManager.instance.PlaySound(sprintingSound, gameObject.transform);
                    ChangeSprintingFootSteps();
                }
                else
                {
                    footstepTimer = timeBetweenWalkingFootsteps;
                    //AudioManager.instance.PlaySound(walkingSound, gameObject.transform);
                    ChangeWalkingFootSteps();
                }
            }
        }

        else
        {
            if (PlMove.isCrouching) footstepTimer = timeBetweenCrouchingFootsteps;

            else if (PlMove.isSprinting) footstepTimer = timeBetweenSprintingFootsteps;

            else footstepTimer = timeBetweenWalkingFootsteps;
        }
    }



    public void ChangeWalkingFootSteps()
    {
        if(Physics.Raycast(RayStart.position, RayStart.transform.up * -1, out FootStepRay, range, GroundLayer))
        {
            //Debug.Log(FootStepRay);
            
            if (FootStepRay.collider.CompareTag("Crypt Floor"))
            {
                //Debug.Log("Walking on Crypt");
                AudioManager.instance.PlaySound(CryptWalking, gameObject.transform);
            }
            else if (FootStepRay.collider.CompareTag("Carpet"))
            {
                //Debug.Log("Walking on Carpet");
                AudioManager.instance.PlaySound(CarpetWalking, gameObject.transform);
            }
            else
            {
                //Debug.Log("Walking on Stone Floor");
                AudioManager.instance.PlaySound(StoneFloorWalking, gameObject.transform);
            }

        }   
    }

    public void ChangeSprintingFootSteps()
    {
        if (Physics.Raycast(RayStart.position, RayStart.transform.up * -1, out FootStepRay, range, GroundLayer))
        {
            if(FootStepRay.collider.CompareTag("Crypt Floor"))
            {
                //Debug.Log("Walking on Crypt");
                AudioManager.instance.PlaySound(CryptSprinting, gameObject.transform);
            }
            else if (FootStepRay.collider.CompareTag("Carpet"))
            {
                //Debug.Log("Walking on Carpet");
                AudioManager.instance.PlaySound(CarpetSprinting, gameObject.transform);
            }
            else
            {
                //Debug.Log("Walking on Stone Floor");
                AudioManager.instance.PlaySound(StoneFloorSprinting, gameObject.transform);
            }
        }
    }

    public void ChangeCrouchingFootSteps()
    {
        if (Physics.Raycast(RayStart.position, RayStart.transform.up * -1, out FootStepRay, range, GroundLayer))
        {
            if (FootStepRay.collider.CompareTag("Crypt Floor"))
            {
                //Debug.Log("Walking on Crypt");
                AudioManager.instance.PlaySound(CryptCrouching, gameObject.transform);
            }
            else if (FootStepRay.collider.CompareTag("Carpet"))
            {
                //Debug.Log("Walking on Carpet");
                AudioManager.instance.PlaySound(CarpetCrouching, gameObject.transform);
            }
            else
            {
                //Debug.Log("Walking on Stone Floor");
                AudioManager.instance.PlaySound(StoneFloorCrouching, gameObject.transform);
            }
        }
    }
}

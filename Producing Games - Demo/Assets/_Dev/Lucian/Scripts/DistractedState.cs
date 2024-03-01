/*Distracted state responsible for making the demon investigate
 a certain sound when he hears it
Made by Lucian*/


using Unity.VisualScripting;
using UnityEngine;

public class DistractedState : DemonStateBaseClass
{
    float investigationTime = 10;
    float currentTime;
    bool startTimer = false;

    private void Start()
    {
        
        currentTime = investigationTime;
    }

    public override void UpdateLogic()
    {
        //if far away from sound the destination will be set to move towards that sound
        if(Mathf.Abs(character.agent.gameObject.transform.position.magnitude - character.soundDestination.position.magnitude) < 5f )
        {
            character.agent.SetDestination(character.soundDestination.position);
            if(Mathf.Abs(character.agent.gameObject.transform.position.magnitude - character.soundDestination.position.magnitude) < 0.1f)
            {
                character.agent.velocity = Vector3.zero;
                startTimer = true;
            }
        }
        else
        {
            character.agent.velocity = Vector3.zero;
            startTimer = true;
        }
        if(startTimer)
        {
            character.animator.SetBool("isConfused", true);
            if(currentTime < 0)
            {
                character.animator.SetBool("isConfused", false);
                character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            }
            else
            {
                currentTime -= Time.deltaTime;
            }

        }
    }
}

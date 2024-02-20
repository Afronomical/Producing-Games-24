/*Distracted state responsible for making the demon investigate
 a certain sound when he hears it
Made by Lucian*/


using Unity.VisualScripting;
using UnityEngine;

public class DistractedState : DemonStateBaseClass
{
    float investigationTime = 5f;
    float currentTime;
    bool startTimer = false;

    private void Start()
    {
        character.agent.SetDestination(character.soundDestination.position);
        currentTime = investigationTime;
    }

    public override void UpdateLogic()
    {
        if(Mathf.Abs(character.agent.gameObject.transform.position.magnitude - character.soundDestination.position.magnitude) < 1 )
        {
            character.agent.velocity = Vector3.zero;
            startTimer = true;
        }
        if(startTimer)
        {
            if(currentTime < 0)
            {
                character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            }
            else
            {
                currentTime -= Time.deltaTime;
            }

        }
    }
}

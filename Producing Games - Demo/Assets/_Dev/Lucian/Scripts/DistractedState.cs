using UnityEngine;


public class DistractedState : DemonStateBaseClass
{
    private void Start()
    {
        character.agent.SetDestination(character.soundDestination.position);
    }
}

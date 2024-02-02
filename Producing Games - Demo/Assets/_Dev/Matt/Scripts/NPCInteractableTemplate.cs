using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class NPCInteractableTemplate : MonoBehaviour, INPCInteractable
{
    //need reference to AI character 
    public AICharacter character;
    public string ToolTipText = " NPC ";
    private void Start()
    {
        character = GetComponent<AICharacter>();
        
    }

    public string Name => throw new System.NotImplementedException();

    public virtual void Interact()
    {

    }

    public virtual void Escort()
    {
        
    }

    public virtual void Exorcise()
    {
        
    }

    public virtual void Give()
    {
        
    }

    public virtual void Take()
    {
       
    }

    
}

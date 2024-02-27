using UnityEngine;

public class NPCInteractableTemplate : MonoBehaviour, INPCInteractable
{
    // Need reference to patient character 
    public PatientCharacter character;
    public string ToolTipText = " NPC ";

    private void Start()
    {
        character = GetComponent<PatientCharacter>(); 
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

using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// </summary>

public class PossessedState : PatientStateBaseClass
{
    // INFO: Need to implement logic for patient to go into possessed state
    // If ragemode occurs and the demon has not yet been spawned
    private void Start()
    {
        // INFO: Extra security check
        if (character.isPossessed)
        {
            // INFO: Character.bed will need to be changed once we assign rooms to patients
           GameObject go = Instantiate(character.demon.demonPrefab, character.bed.transform.position, Quaternion.identity);
            GameManager.Instance.demon = go;
            
        }
    }

    public override void UpdateLogic()
    {

    }
}

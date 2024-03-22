using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorNPC door;
    private int agentsInRange = 0;

    private DoorInteractable doorInteractable;

    private void Start()
    {
        doorInteractable = transform.GetComponentInChildren<DoorInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            other.GetComponent<Animator>().SetBool("hasOpenedDoor", false);
            other.GetComponent<Animator>().Play("OpenDoorOutwards", 0, 0.0f);

            if (other.TryGetComponent<AICharacter>(out AICharacter character))
                character.isOpeningDoor = true;

            agentsInRange++;

            if (agent.TryGetComponent<DemonCharacter>(out _))
            {
                doorInteractable.Open(other.gameObject.transform.position);
                return;
            }

            if (!door.isOpen)
            {
                door.OpenDoor(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            other.GetComponent<Animator>().SetBool("hasOpenedDoor", true);
        }


        if (other.TryGetComponent<AICharacter>(out AICharacter character))
            character.isOpeningDoor = false;

        //if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        //{
        //    agentsInRange--;
        //    if (door.isOpen && agentsInRange == 0)
        //    {
        //        door.CloseDoor();
        //        //other.GetComponent<Animator>().SetBool("hasOpenedDoor", false);
        //    }
        //}
    }
}

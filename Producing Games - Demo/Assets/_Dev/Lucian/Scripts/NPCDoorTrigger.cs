using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorNPC door;
    private int agentsInRange = 0;

    public AnimationClip animation;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Animator>().SetBool("hasOpenedDoor", false);
        other.GetComponent<Animator>().Play("OpenDoorOutwards", 0, 0.0f);

        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agentsInRange++;
            if (!door.isOpen)
            {
                door.OpenDoor(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<Animator>().SetBool("hasOpenedDoor", true);
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agentsInRange--;
            if (door.isOpen && agentsInRange == 0)
            {
                door.CloseDoor();
                //other.GetComponent<Animator>().SetBool("hasOpenedDoor", false);
            }
        }
    }
}

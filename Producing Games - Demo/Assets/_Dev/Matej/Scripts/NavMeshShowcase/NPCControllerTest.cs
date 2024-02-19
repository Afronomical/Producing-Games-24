using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControllerTest : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray hitLocation = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(hitLocation, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}

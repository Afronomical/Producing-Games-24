using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake
/// Moderated By: .....
/// 
/// Points a raycast from AI to player. Initially for use in escorted state. 
/// </summary>

public class RaycastToPlayer : MonoBehaviour
{
    private AICharacter character;
    public float detectionRange = 40f;
    public LayerMask UnwalkableLayer;
    public float PlayerDistance; 
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<AICharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(character.player.transform.position);
        PlayerDistance = Vector3.Distance(character.transform.position ,character.player.transform.position);
    }

    /// <summary>
    /// determines whether the player can be seen by the NPCS. 
    /// </summary>
    /// <returns></returns>
   public bool PlayerDetected()
    {
        //Vector3 playerDir = (character.player.transform.position - character.transform.position);
        
        Ray ray = new Ray(character.transform.position, transform.forward);

        RaycastHit hitInfo; 
        if(Physics.Raycast(ray, out hitInfo,detectionRange,UnwalkableLayer))
        {
            Debug.DrawLine(ray.origin, hitInfo.point,Color.red);
            Debug.Log("Obstacle Detected: " + hitInfo.collider.gameObject.name);
            return false;
        }
        else
        {
            Debug.DrawLine(ray.origin,ray.origin + ray.direction * detectionRange,Color.green);
           
           
            if (PlayerDistance <= detectionRange)
            {
                Debug.Log("Player Detected at distance");
                return true;
            }

           
        }
        //Debug.Log("returning as false");
        return false;
        
    }
}

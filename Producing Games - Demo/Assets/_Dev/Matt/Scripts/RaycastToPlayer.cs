using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: .....
/// <para>Points a raycast from AI to player. Initially for use in escorted state.  </para>
/// </summary>

public class RaycastToPlayer : MonoBehaviour
{
    private AICharacter character;
   [ShowOnly][SerializeField] private float detectionRange = 10f;
    public LayerMask UnwalkableLayer;
   [ShowOnly] public float PlayerDistance; 
    
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
            float distToPlayer = Vector3.Distance(character.transform.position, character.player.transform.position);
            if(hitInfo.distance < distToPlayer)
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                Debug.Log("Obstacle Detected: " + hitInfo.collider.gameObject.name);
                return false;
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);
                return true; 
            }
            
            
        }
        else if(PlayerDistance <= detectionRange)
        {
            Debug.DrawLine(ray.origin,ray.origin + ray.direction * detectionRange,Color.green);
            Debug.Log("Player Detected within range");
            return true;
           
            //if (PlayerDistance <= detectionRange)
            //{
            //    Debug.Log("Player Detected within range");
            //    return true;
            //}


        }
        //Debug.Log("returning as false");
        return false;
        
    }
}

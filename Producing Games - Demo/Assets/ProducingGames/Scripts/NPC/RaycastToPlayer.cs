using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: Matej Cincibus
/// <para>Points a raycast from AI to player. Initially for use in escorted state.  </para>
/// </summary>

public class RaycastToPlayer : MonoBehaviour
{
    private AICharacter character;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask unwalkableLayer;
    [ShowOnly] public float playerDistance;

    private void Awake()
    {
        character = GetComponent<AICharacter>();
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(character.transform.position, character.player.transform.position);
    }

    /// <summary>
    /// determines whether the player can be seen by the NPCS. 
    /// </summary>
    /// <returns></returns>
    public bool PlayerDetected()
    {
        Ray ray = new(character.transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, detectionRange, unwalkableLayer))
        {
            float distToPlayer = Vector3.Distance(character.transform.position, character.player.transform.position);
            if (hitInfo.distance < distToPlayer)
            {
                //Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                //Debug.Log("Obstacle Detected: " + hitInfo.collider.gameObject.name);
                return false;
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);
                ///obstacle in range but behind player 
                return true;
            }
        }
        else if (playerDistance <= detectionRange)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);
            //Debug.Log("Player Detected within range");

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

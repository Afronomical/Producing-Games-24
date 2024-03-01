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
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                return false;
            }
            else
            {
                // obstacle in range but behind player 
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);
                return true;
            }
        }
        else if (playerDistance <= detectionRange)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.red);
            return true;
        }
        return false;
    }
}

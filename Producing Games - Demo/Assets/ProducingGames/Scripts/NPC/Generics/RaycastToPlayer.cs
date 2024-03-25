using System;
using System.Collections;
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

    public float radius;
    [Range(0,360)]
    public float angle;

    private GameObject playerRef;

    public bool canSeePlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private void Awake()
    {
        character = GetComponent<AICharacter>();
        playerRef = character.player;
    }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(character.transform.position, GameManager.Instance.player.transform.position);
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

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;

        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            LookForPlayer();
        }
    }

    public bool LookForPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < angle / 2)
            {
                float distToPlayer = Vector3.Distance(character.transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToPlayer, unwalkableLayer))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else 
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        return canSeePlayer;
    }
}

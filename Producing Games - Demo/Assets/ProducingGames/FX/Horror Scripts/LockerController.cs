using UnityEngine;

public class LockerController : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float activationDistance = 2f; // Distance at which the animation should start playing
    public float animationDuration = 2f; // Duration for which the animation should play

    private Animator animator;
    private bool isPlayerNear = false;
    private bool isAnimationPlaying = false;
    private float animationTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the player is within the activation distance
        if (Vector3.Distance(transform.position, player.position) <= activationDistance)
        {
            isPlayerNear = true;
        }
        else
        {
            isPlayerNear = false;
        }

        // If the player is near and the animation is not already playing
        if (isPlayerNear && !isAnimationPlaying)
        {
            // Play the animation
            animator.SetTrigger("OpenCloseTrigger");
            isAnimationPlaying = true;

            // Start the timer for animation duration
            animationTimer = animationDuration;
        }

        // If the animation is currently playing and timer is greater than 0
        if (isAnimationPlaying && animationTimer > 0f)
        {
            animationTimer -= Time.deltaTime;
        }
        else if (isAnimationPlaying && animationTimer <= 0f)
        {
            // Stop the animation when timer reaches 0
            animator.SetTrigger("IdleTrigger");
            isAnimationPlaying = false;
        }
    }
}



using UnityEngine;

public class LockerController : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float activationDistance = 2f; // Distance at which the animation should start playing
    public float animationDuration = 2f; // Duration for which the animation should play

    private Animator animator;
    private bool isAnimationPlaying = false;
    private float animationTimer = 0f;
    private GameManager gM;
    private bool eventTriggered;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gM = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAnimationPlaying && Random.Range(0, 101) <= gM.eventChance && !eventTriggered)
        {
            // Play the animation
            animator.SetTrigger("OpenCloseTrigger");
            isAnimationPlaying = true;

            // Start the timer for animation duration
            animationTimer = animationDuration;

            eventTriggered = true;
        }
    }

    private void Update()
    {
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

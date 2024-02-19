using UnityEngine;

public class DrawerEvent : MonoBehaviour
{
    public AnimationClip drawerAnimationClip;
    public float triggerDistance = 2f;
    public AudioClip openingClosingSound;

    private bool isPlayerNearby = false;
    private Animation drawerAnimation;
    private AudioSource audioSource;

    void Start()
    {
        drawerAnimation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E)) // You can change the input key as needed
            {
                if (!drawerAnimation.isPlaying)
                {
                    PlayAnimation();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void PlayAnimation()
    {
        if (drawerAnimation != null)
        {
            if (drawerAnimation.isPlaying)
            {
                drawerAnimation.Stop();
            }

            if (drawerAnimationClip == null)
            {
                Debug.LogWarning("No animation clip assigned to the drawer.");
                return;
            }

            drawerAnimation.clip = drawerAnimationClip;
            drawerAnimation.Play();

            if (openingClosingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(openingClosingSound);
            }
        }
    }
}


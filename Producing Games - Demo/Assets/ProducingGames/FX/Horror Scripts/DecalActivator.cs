using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalActivator : MonoBehaviour
{
    public GameObject[] decalProjectors;
    public float activationTimeInMinutes = 3f; // Time to wait before activating decals in minutes

    private bool allDecalsActivated = false;

    private void Start()
    {
        // Deactivate all decal projectors initially
        foreach (GameObject projector in decalProjectors)
        {
            projector.SetActive(false);
        }

        // Start the coroutine to activate decals after a delay
        StartCoroutine(ActivateDecalsAfterDelay());
    }

    private System.Collections.IEnumerator ActivateDecalsAfterDelay()
    {
        // Wait for the specified activation time
        yield return new WaitForSeconds(activationTimeInMinutes * 60); // Convert minutes to seconds

        // Activate decals based on the event chance
        ActivateDecals();
    }

    private void ActivateDecals()
    {
        // Get the event chance from the GameManager
        int eventChance = GameManager.Instance.eventChance;

        // Generate a random number between 0 and 100
        int randomValue = Random.Range(0, 100);

        // Check if the random value is less than the event chance
        if (randomValue < eventChance)
        {
            // Choose a random index within the decalProjectors array
            int randomIndex = Random.Range(0, decalProjectors.Length);

            // Activate the decal projector at the chosen random index
            decalProjectors[randomIndex].SetActive(true);

            // Check if all decal projectors are activated
            CheckAllDecalsActivated();
        }
    }

    private void CheckAllDecalsActivated()
    {
        // Check if all decal projectors are activated
        foreach (GameObject projector in decalProjectors)
        {
            if (!projector.activeSelf)
                return; // If any projector is not activated, exit the function
        }

        // If all projectors are activated, set the allDecalsActivated flag to true
        allDecalsActivated = true;
    }
}

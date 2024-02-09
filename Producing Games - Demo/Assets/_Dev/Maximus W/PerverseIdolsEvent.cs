using UnityEngine;
using System.Collections;

public class PerverseIdolsEvent : MonoBehaviour
{
    public GameObject perverseIdolsObject;
    public float duration = 20f; // Duration of the perverse idols event

    private void Start()
    {
        TriggerPerverseIdols();
    }

    private void TriggerPerverseIdols()
    {
        StartCoroutine(PerverseIdolsCoroutine());
    }

    private IEnumerator PerverseIdolsCoroutine()
    {
        perverseIdolsObject.SetActive(true);
        // Add any additional logic or animation for the perverse idols event

        yield return new WaitForSeconds(duration);

        perverseIdolsObject.SetActive(false);
        // Additional logic to reset or clean up after the perverse idols event
    }
}
using UnityEngine;
using System.Collections;

public class BloodWritingEvent : MonoBehaviour
{
    public GameObject bloodWritingObject;
    public float duration = 10f; // Duration of the blood writing event

    private void Start()
    {
        TriggerBloodWriting();
    }

    // Change the access modifier to public
    public void TriggerBloodWriting()
    {
        StartCoroutine(BloodWritingCoroutine());
    }

    private IEnumerator BloodWritingCoroutine()
    {
        bloodWritingObject.SetActive(true);
        // Add any additional logic or animation for the blood writing event

        yield return new WaitForSeconds(duration);

        bloodWritingObject.SetActive(false);
        // Additional logic to reset or clean up after the blood writing event
    }
}

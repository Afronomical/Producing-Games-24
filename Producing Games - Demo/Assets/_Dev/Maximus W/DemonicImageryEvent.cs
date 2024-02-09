using UnityEngine;
using System.Collections;
public class DemonicImageryEvent : MonoBehaviour
{
    public GameObject demonicImageryObject;
    public float duration = 15f; // Duration of the demonic imagery event

    private void Start()
    {
        TriggerDemonicImagery();
    }

    // Change the access modifier to public
    public void TriggerDemonicImagery()
    {
        StartCoroutine(DemonicImageryCoroutine());
    }

    private IEnumerator DemonicImageryCoroutine()
    {
        demonicImageryObject.SetActive(true);
        // Add any additional logic or animation for the demonic imagery event

        yield return new WaitForSeconds(duration);

        demonicImageryObject.SetActive(false);
        // Additional logic to reset or clean up after the demonic imagery event
    }
}
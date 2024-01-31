using System.Collections;
using UnityEngine;

public class HotBarEffect : MonoBehaviour
{
    public RectTransform panel; // Reference to your public panel
    public float increaseSpeed = 1f;  // Adjusted for slower increase
    public float decreaseSpeed = 1f;  // Adjusted for slower decrease
    public float waitDuration = 5f;  // Adjusted for a 5-second wait duration

    private Vector3 originalSize;
    private bool isIncreasing = false;

    private void Start()
    {
        // Store the original size
        originalSize = panel.localScale;

        // Subscribe to the event when an item is picked up
        InventoryHotbar.instance.OnItemPickedUp += StartIncreaseEffect;
        InventoryHotbar.instance.OnItemSelected += StartIncreaseEffect;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the script is destroyed
        InventoryHotbar.instance.OnItemPickedUp -= StartIncreaseEffect;
        InventoryHotbar.instance.OnItemSelected -= StartIncreaseEffect;
    }

    // Coroutine to gradually increase and decrease the size
    private IEnumerator SizeEffectCoroutine()
    {
        // Increase the size to twice the original size
        float elapsedTime = 0f;
        Vector3 targetSize = originalSize * 1.5f;

        while (elapsedTime < 1f)
        {
            panel.localScale = Vector3.Lerp(originalSize, targetSize, elapsedTime);
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        // Set the size to the target size to avoid inaccuracies
        panel.localScale = targetSize;

        // Wait for a longer duration only if there is no input during the waitDuration
        float timer = 0f;
        while (timer < waitDuration)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                // If there is input, reset the timer
                timer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Decrease the size back to the original size
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            panel.localScale = Vector3.Lerp(targetSize, originalSize, elapsedTime);
            elapsedTime += Time.deltaTime * decreaseSpeed;
            yield return null;
        }

        // Set the size back to the original size to avoid inaccuracies
        panel.localScale = originalSize;

        // Move the isIncreasing flag outside the if block
        isIncreasing = false;
    }

    // Callback when an item is picked up
    private void StartIncreaseEffect()
    {
        // Check if the coroutine is not already running
        if (!isIncreasing)
        {
            isIncreasing = true;
            StartCoroutine(SizeEffectCoroutine());
        }
    }
}
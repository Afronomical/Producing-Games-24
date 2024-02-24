using UnityEngine;
using UnityEngine.UI;

public class CanvasResolutionController : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    private void Start()
    {
        // Set initial reference resolution
        UpdateCanvasScalerReferenceResolution(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    private void Update()
    {
        // Check if the current resolution is different from the CanvasScaler reference resolution
        if (canvasScaler != null &&
            (int)canvasScaler.referenceResolution.x != Screen.currentResolution.width ||
            (int)canvasScaler.referenceResolution.y != Screen.currentResolution.height)
        {
            // Update the CanvasScaler reference resolution
            UpdateCanvasScalerReferenceResolution(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }

    private void UpdateCanvasScalerReferenceResolution(int width, int height)
    {
        // Update the CanvasScaler reference resolution based on the new resolution
        if (canvasScaler != null)
        {
            canvasScaler.referenceResolution = new Vector2(width, height);
            Debug.Log($"CanvasScaler reference resolution updated to {width}x{height}");
        }
    }
}

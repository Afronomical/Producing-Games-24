using UnityEngine;

public class CameraToMaterial : MonoBehaviour
{
    public Camera targetCamera; // Assign the camera you want to capture in the Inspector
    public Material outputMaterialPrefab; // Assign the material prefab you created in the Inspector

    private static Material outputMaterial;

    void Start()
    {
        if (outputMaterial == null)
        {
            // If the material doesn't exist yet, create and store it
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            targetCamera.targetTexture = renderTexture;
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            targetCamera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            targetCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            outputMaterial = new Material(outputMaterialPrefab);
            outputMaterial.mainTexture = screenShot;

            // Make the GameObject persist across scenes
            DontDestroyOnLoad(gameObject);
        }

        // Apply the material to the quad or any other object you want
        GetComponent<Renderer>().material = outputMaterial;
    }
}
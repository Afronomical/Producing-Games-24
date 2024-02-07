using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    public TextMeshProUGUI fpsDisplayText; // Reference to the Text component for displaying FPS.
    private bool isFPSToggled = false;
    private float FPSdeltaTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // // FPS Code // //

        // Calculate FPS
        FPSdeltaTime += (Time.unscaledDeltaTime - FPSdeltaTime) * 0.1f;
        float fps = 1.0f / FPSdeltaTime;

        // Display FPS
        fpsDisplayText.text = "FPS: " + Mathf.Ceil(fps).ToString();

    }

    public void ToggleFPSDisplay()
    {
        isFPSToggled = !isFPSToggled;

        if (isFPSToggled)
        {
            // Enable the FPS display
            fpsDisplayText.gameObject.SetActive(true);
        }
        else
        {
            // Disable the FPS display
            fpsDisplayText.gameObject.SetActive(false);
        }
    }
}
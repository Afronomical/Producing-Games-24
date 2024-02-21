using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class AntiAliasingToggle : MonoBehaviour
{
    public HDAdditionalCameraData cameraData;
    public Button toggleButton;

    private bool isAntialiasingActive = true;
    private Color activeColor = Color.green;
    private Color inactiveColor = Color.red;

    void Start()
    {
      

        
        toggleButton.onClick.AddListener(ToggleAntialiasingState);

        //Set initial button color
        UpdateButtonColor();
    }

    void ToggleAntialiasingState()
    {
        //checks wether its set to FXAA. if it is already, then set to none. if FXAA is not active then it will activate.  (just a ternary to toggle between states)  
        cameraData.antialiasing = (cameraData.antialiasing == HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing)
            ? HDAdditionalCameraData.AntialiasingMode.None
            : HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;

        // Update button color
        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        isAntialiasingActive = (cameraData.antialiasing == HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing);

        //Change button color based on the state
        toggleButton.image.color = isAntialiasingActive ? activeColor : inactiveColor;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class FOVSlider : MonoBehaviour
{
    float defaultFovValue;

    void Start()
    {
        //Set slider value based on the current FOV
        SettingsManager.Instance.fovSlider.value = SettingsManager.Instance.mainCamera.fieldOfView;
        defaultFovValue = 75;


        //listener for slider value changes
        SettingsManager.Instance.fovSlider.onValueChanged.AddListener(OnFOVSliderValueChanged);
        SettingsManager.Instance.defaultFov.onClick.AddListener(OnDefaultFovClicked);
    }

    //Called when slider value changes
    private void OnFOVSliderValueChanged(float newValue)
    {
        //Update the cameras FOV
        SettingsManager.Instance.mainCamera.fieldOfView = newValue;
    }

    private void OnDefaultFovClicked()
    {
        SettingsManager.Instance.fovSlider.value = defaultFovValue;
    }
}
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class AntiAliasingSetting : MonoBehaviour
{
    public HDAdditionalCameraData cameraData;
    public TMP_Dropdown antiAliasingDropdown;

    private List<string> customNames = new List<string>
    {
        "None",
        "FXAA", //Fast Approximate Antialiasing
        "TAA", //Temporal Antialiasing
        "SMAA" //Subpixel Morphological Antialiasing
    };

    void Start()
    {
        if (antiAliasingDropdown != null)
        {
            //Clear options in dropdown
            antiAliasingDropdown.ClearOptions();

            //custom names to the dropdown
            antiAliasingDropdown.AddOptions(customNames);

            antiAliasingDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            UpdateDropdownValue();
        }
    }

    void OnDropdownValueChanged(int value)
    {
        //Convert the dropdown value to AntialiasingMode enum
        HDAdditionalCameraData.AntialiasingMode selectedMode = (HDAdditionalCameraData.AntialiasingMode)value;

        cameraData.antialiasing = selectedMode;

        ApplyAntiAliasingSettings();
    }

    void UpdateDropdownValue()
    {
        antiAliasingDropdown.value = (int)cameraData.antialiasing;
    }

    //Just incase for applying any additional settings. not acc necessary and doesnt apply anything right now
    void ApplyAntiAliasingSettings()
    {
        switch (cameraData.antialiasing)
        {
            case HDAdditionalCameraData.AntialiasingMode.None:
                break;
            case HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing:
                break;
            case HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing:
                break;
            case HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing://REMINDER**** This one has a capital A in Aliasing.... :'( ******
             
             //Example of setting addition: cameraData.subpixelMorphologicalAntialiAsing.quality = SubpixelMorphologicalAntialiAsing.Quality.High;
                break;
        }
    }
}
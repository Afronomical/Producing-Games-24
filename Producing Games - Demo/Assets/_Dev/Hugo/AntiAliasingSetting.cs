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

        if (SettingsSceneValues.Instance.newAntiNone)
        {
            antiAliasingDropdown.value = customNames.IndexOf("None");
        }
        else if (SettingsSceneValues.Instance.newAntiFXAA)
        {
            antiAliasingDropdown.value = customNames.IndexOf("FXAA");
        }
        else if (SettingsSceneValues.Instance.newAntiTAA)
        {
            antiAliasingDropdown.value = customNames.IndexOf("TAA");
        }
        else if (SettingsSceneValues.Instance.newAntiSMAA)
        {
            antiAliasingDropdown.value = customNames.IndexOf("SMAA");
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

    //Just incase for applying any additional settings. 
    void ApplyAntiAliasingSettings()
    {
        switch (cameraData.antialiasing)
        {
            case HDAdditionalCameraData.AntialiasingMode.None:
                SettingsSceneValues.Instance.newAntiNone = true;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;
            case HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing:
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = true;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;
            case HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing:
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = true;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;
            case HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing:  //REMINDER**** This one has a capital A in Aliasing.... :'( ******
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = true;
             
             //Example of setting addition: cameraData.subpixelMorphologicalAntialiAsing.quality = SubpixelMorphologicalAntialiAsing.Quality.High;
                break;
        }
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;



//Initial colorblind settings using the channel mixer. not 100% accurate but a good starting point. 
//potenitially will be improved with better methods of applying colourblindness settings instead of using channel mixer. shaders?
public class ColorBlindnessSetting: MonoBehaviour
{
    public Volume colorBlindnessVolume;
    public TMP_Dropdown colorBlindnessDropdown;

    private ChannelMixer channelMixerSettings;

    public enum ColorBlindnessMode
    {
        Normal,
        Protanopia,
        Deuteranopia,
        Tritanopia
    }

    private void Start()
    {
        //setting up the dropdown
        colorBlindnessDropdown.ClearOptions();
        colorBlindnessDropdown.AddOptions(new List<string> { "Normal", "Protanopia", "Deuteranopia", "Tritanopia" });

        //Get Channel Mixer settings
        if (colorBlindnessVolume.profile.TryGet(out channelMixerSettings))
        {
           
            colorBlindnessDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
      
    }

    private void OnDropdownValueChanged(int value)
    {
        ColorBlindnessMode mode = (ColorBlindnessMode)value;
        SetColorBlindnessMode(mode);
    }

    private void SetColorBlindnessMode(ColorBlindnessMode mode)
    {
        switch (mode)
        {
            //call function depending on which mode selected
            case ColorBlindnessMode.Normal:
                ResetChannelMixerSettings();
                break;

            case ColorBlindnessMode.Protanopia:
                SetProtanopiaChannelMixerSettings();
                break;

            case ColorBlindnessMode.Deuteranopia:
                SetDeuteranopiaChannelMixerSettings();
                break;

            case ColorBlindnessMode.Tritanopia:
                SetTritanopiaChannelMixerSettings();
                break;
        }
    }


    //based of minimal research. needs more looking into to set correct colours. 
    
    //functions to swap channels.
    private void ResetChannelMixerSettings()
    {
        //Reset to normal
        channelMixerSettings.redOutRedIn.value = 100f;
        channelMixerSettings.greenOutRedIn.value = 0f;
        channelMixerSettings.blueOutRedIn.value = 0f;

        channelMixerSettings.redOutGreenIn.value = 0f;
        channelMixerSettings.greenOutGreenIn.value = 100f;
        channelMixerSettings.blueOutGreenIn.value = 0f;

        channelMixerSettings.redOutBlueIn.value = 0f;
        channelMixerSettings.greenOutBlueIn.value = 0f;
        channelMixerSettings.blueOutBlueIn.value = 100f;
    }


    private void SetProtanopiaChannelMixerSettings()
    {

        channelMixerSettings.redOutRedIn.value = 0f;
        channelMixerSettings.greenOutRedIn.value = 100f;
        channelMixerSettings.blueOutRedIn.value = 0f;

        channelMixerSettings.redOutGreenIn.value = 100f;
        channelMixerSettings.greenOutGreenIn.value = 0f;
        channelMixerSettings.blueOutGreenIn.value = 0f;

        channelMixerSettings.redOutBlueIn.value = 0f;
        channelMixerSettings.greenOutBlueIn.value = 0f;
        channelMixerSettings.blueOutBlueIn.value = 100f;
    }

    private void SetDeuteranopiaChannelMixerSettings()
    { 
        channelMixerSettings.redOutRedIn.value = 0f;
        channelMixerSettings.greenOutRedIn.value = 0f;
        channelMixerSettings.blueOutRedIn.value = 100f;

        channelMixerSettings.redOutGreenIn.value = 100f;
        channelMixerSettings.greenOutGreenIn.value = 0f;
        channelMixerSettings.blueOutGreenIn.value = 0f;

        channelMixerSettings.redOutBlueIn.value = 0f;
        channelMixerSettings.greenOutBlueIn.value = 100f;
        channelMixerSettings.blueOutBlueIn.value = 0f;
    }

    private void SetTritanopiaChannelMixerSettings()
    {
        channelMixerSettings.redOutRedIn.value = 100f;
        channelMixerSettings.greenOutRedIn.value = 0f;
        channelMixerSettings.blueOutRedIn.value = 0f;

        channelMixerSettings.redOutGreenIn.value = 0f;
        channelMixerSettings.greenOutGreenIn.value = 0f;
        channelMixerSettings.blueOutGreenIn.value = 100f;

        channelMixerSettings.redOutBlueIn.value = 0f;
        channelMixerSettings.greenOutBlueIn.value = 100f;
        channelMixerSettings.blueOutBlueIn.value = 0f;
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; 
using System.Collections.Generic;
using System;



public class AccessibilitySettingsManager : MonoBehaviour
{
    [Header("Inventory Elements")]
    public RectTransform inventoryPanel;
    public float increaseEffectSpeed = 3f;
    public float decreaseEffectSpeed = 3f;
    public float effectWaitDuration = 1.5f;
    public float effectScaleMultiplier = 1.2f;
    public Slider inventoryScaleSlider;
    [HideInInspector] public Vector3 originalInventorySize;
    [HideInInspector] public bool isInventoryIncreasing = false;
    [HideInInspector] public CanvasGroup inventoryPanelCanvasGroup;
    [HideInInspector] public float originalInventoryAspect;
    public GameObject demonBook;
    public GameObject checkList;

    [Header("Colour Blindness Elements")]
    public Volume colorBlindnessVolume;
    public TMP_Dropdown colorBlindnessDropdown;
    private ChannelMixer channelMixerSettings;


    //Default values
    private float defaultIncreaseEffectSpeed = 3f;
    private float defaultDecreaseEffectSpeed = 3f;
    private float defaultEffectWaitDuration = 1.5f;
    private float defaultEffectScaleMultiplier = 1.2f;
    private float defaultMotionBlurIntensity = 0.5f; // Assuming default intensity
    private ColorBlindnessMode defaultColorBlindnessMode = ColorBlindnessMode.Normal;


    public enum ColorBlindnessMode
    {
        Normal,
        Protanopia,
        Deuteranopia,
        Tritanopia
    }

    
    [Header("Motion Blur Elements")]
    public Volume volume;
    public Slider motionBlurSlider;
    private MotionBlur motionBlurVolume; 

    private static AccessibilitySettingsManager _instance;

    public static AccessibilitySettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("AccessibilitySettingsManager").AddComponent<AccessibilitySettingsManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

        /*
        ************************************
        **********Colour Blindness**********
        ************************************
        */
        //setting up the dropdown
        colorBlindnessDropdown.ClearOptions();
        colorBlindnessDropdown.AddOptions(new List<string> { "Normal", "Protanopia", "Deuteranopia", "Tritanopia" });

        //Get Channel Mixer settings
        if (colorBlindnessVolume.profile.TryGet(out channelMixerSettings))
        {
           
            colorBlindnessDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }


        /*
        ************************************
        ************Motion Blur*************
        ************************************
        */

        if (volume.profile.TryGet(out motionBlurVolume))
        {
            UpdateMotionBlurIntensity();
        }
        motionBlurSlider.onValueChanged.AddListener(OnMotionBlurSliderChanged);







        motionBlurVolume.intensity.value = SettingsSceneValues.Instance.motionBlurNew;
        motionBlurSlider.value = SettingsSceneValues.Instance.motionBlurNew;
        
        if (SettingsSceneValues.Instance.newNormal == true)
        {
            colorBlindnessDropdown.value = (int)ColorBlindnessMode.Normal;

        }

        if (SettingsSceneValues.Instance.newProtanopia == true)
        {
            colorBlindnessDropdown.value = (int)ColorBlindnessMode.Protanopia;

        }
        
        if (SettingsSceneValues.Instance.newDeuteranopia == true)
        {
            colorBlindnessDropdown.value = (int)ColorBlindnessMode.Deuteranopia;

        }

        if (SettingsSceneValues.Instance.newTritanopia == true)
        {
            colorBlindnessDropdown.value = (int)ColorBlindnessMode.Tritanopia;

        }
       

       
        
    }

   
   
   
    /*
        ************************************************
        **************    FUNCTIONS    *****************
        ************************************************
    */



    /*
        ************************************
        **********Colour Blindness**********
        ************************************
    */
#region ColourBlindess
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
                SettingsSceneValues.Instance.newNormal = true;
                SettingsSceneValues.Instance.newProtanopia = false;
                SettingsSceneValues.Instance.newDeuteranopia = false;
                SettingsSceneValues.Instance.newTritanopia = false;
                ResetChannelMixerSettings();
                break;

            case ColorBlindnessMode.Protanopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = true;
                SettingsSceneValues.Instance.newDeuteranopia = false;
                SettingsSceneValues.Instance.newTritanopia = false;
                SetProtanopiaChannelMixerSettings();
                break;

            case ColorBlindnessMode.Deuteranopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = false;
                SettingsSceneValues.Instance.newDeuteranopia = true;
                SettingsSceneValues.Instance.newTritanopia = false;
                SetDeuteranopiaChannelMixerSettings();
                break;

            case ColorBlindnessMode.Tritanopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = false;
                SettingsSceneValues.Instance.newDeuteranopia = false;
                SettingsSceneValues.Instance.newTritanopia = true;
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

#endregion



    /*
        ************************************
        ************Motion Blur*************
        ************************************
    */
#region MotionBlur

private void OnMotionBlurSliderChanged(float value)
    {
        if (motionBlurVolume != null)
        {
            //update the motion blur setting in volume with value from slider
            motionBlurVolume.intensity.value = value;
            SettingsSceneValues.Instance.motionBlurNew = value;
        }
    }

    

    //set slider value to motionblur intensity when called in start
    private void UpdateMotionBlurIntensity()
    {
        if (motionBlurVolume != null)
        {
            motionBlurSlider.value = motionBlurVolume.intensity.value;
        }
    }

    #endregion


    // Function to set default values for all settings
    private void SetDefaultValues()
    {
        increaseEffectSpeed = defaultIncreaseEffectSpeed;
        decreaseEffectSpeed = defaultDecreaseEffectSpeed;
        effectWaitDuration = defaultEffectWaitDuration;
        effectScaleMultiplier = defaultEffectScaleMultiplier;
        motionBlurSlider.value = defaultMotionBlurIntensity;
        colorBlindnessDropdown.value = (int)defaultColorBlindnessMode;
        SetColorBlindnessMode(defaultColorBlindnessMode); // Update color blindness settings to match default mode
    }

    // Function to update visuals of menus, sliders, and buttons
    private void UpdateVisuals()
    {
        // Update inventory scale slider
        inventoryScaleSlider.value = CalculateSliderValue(increaseEffectSpeed);

        // Update motion blur slider
        motionBlurSlider.value = defaultMotionBlurIntensity;

        // Update color blindness dropdown
        colorBlindnessDropdown.value = (int)defaultColorBlindnessMode;
    }

    // Function to reset all settings to their defaults
    public void ResetAccessibilitySettingsToDefaults()
    {
        SetDefaultValues();
        UpdateVisuals();
    }

    private float CalculateSliderValue(float speed)
    {
        // Logic to convert speed to slider value based on some scaling factor
        // Modify this according to your requirements
        return speed / 10f;
    }

}


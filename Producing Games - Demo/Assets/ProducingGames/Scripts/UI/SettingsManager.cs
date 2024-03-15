using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; 
using System.Collections.Generic;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public Slider motionBlurSlider;
   

    public Slider Fov;
   

    public TMP_Dropdown colorBlindnessDropdown;

    public enum ColorBlindnessMode
    {
        Normal,
        Protanopia,
        Deuteranopia,
        Tritanopia
    }

    public TMP_Dropdown antiAliasingDropdown;

    public enum AntiAliasingMode
    {
        None,
        FXAA,
        TAA,
        SMAA
    }

    public Slider volume;
   
    [SerializeField] private AudioManager audioManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    

    public void MotionBlurEnable()
    {
        // Attempt to find the slider
        GameObject sliderObject = GameObject.Find("MotionBlurSlider");

        if (sliderObject != null)
        {
            motionBlurSlider = sliderObject.GetComponent<Slider>();

            if (motionBlurSlider != null)
            {
                // Add listener
                motionBlurSlider.onValueChanged.AddListener(OnMotionBlurSliderChanged);
                motionBlurSlider.value = SettingsSceneValues.Instance.motionBlurNew;
            }
            else
            {
                Debug.LogError("MotionBlurSlider component not found on the object named 'MotionBlurSlider'");
            }
        }
        else
        {
            Debug.LogError("Object named 'MotionBlurSlider' not found in the scene.");
        }
    }

    private void OnMotionBlurSliderChanged(float value)
    {
        SettingsSceneValues.Instance.motionBlurNew = motionBlurSlider.value;
    }



    public void FovEnable()
    {

        // Attempt to find the slider
        GameObject sliderObject = GameObject.Find("FOV");

        if (sliderObject != null)
        {
            Fov = sliderObject.GetComponent<Slider>();

            if (Fov != null)
            {
                // Add listener
                Fov.onValueChanged.AddListener(OnFovSliderChanged);
                Fov.value = SettingsSceneValues.Instance.FovNew;
            }
            else
            {
                Debug.LogError("MotionBlurSlider component not found on the object named 'MotionBlurSlider'");
            }
        }
        else
        {
            Debug.LogError("Object named 'MotionBlurSlider' not found in the scene.");
        }

    }

    private void OnFovSliderChanged(float value)
    {
        SettingsSceneValues.Instance.FovNew = Fov.value;
    }

    public void ColourBlindnessEnable()
    {
        GameObject ColourObj = GameObject.Find("ColourBlindessDropdown");
        colorBlindnessDropdown = ColourObj.GetComponent<TMP_Dropdown>();

        if (colorBlindnessDropdown != null)
        {
            colorBlindnessDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            colorBlindnessDropdown.ClearOptions();
            colorBlindnessDropdown.AddOptions(new List<string> { "Normal", "Protanopia", "Deuteranopia", "Tritanopia" });

        }


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
                break;

            case ColorBlindnessMode.Protanopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = true;
                SettingsSceneValues.Instance.newDeuteranopia = false;
                SettingsSceneValues.Instance.newTritanopia = false;
                break;

            case ColorBlindnessMode.Deuteranopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = false;
                SettingsSceneValues.Instance.newDeuteranopia = true;
                SettingsSceneValues.Instance.newTritanopia = false;
                break;

            case ColorBlindnessMode.Tritanopia:
                SettingsSceneValues.Instance.newNormal = false;
                SettingsSceneValues.Instance.newProtanopia = false;
                SettingsSceneValues.Instance.newDeuteranopia = false;
                SettingsSceneValues.Instance.newTritanopia = true;
                break;
        }
    }


    public void AntiAliasingEnable()
    {
        GameObject AntiObj = GameObject.Find("AntiAliasingDropdown");
        antiAliasingDropdown = AntiObj.GetComponent<TMP_Dropdown>();

        if (antiAliasingDropdown != null)
        {
            antiAliasingDropdown.onValueChanged.AddListener(OnAntiDropdownValueChanged);
            antiAliasingDropdown.ClearOptions();
            antiAliasingDropdown.AddOptions(new List<string> { "None", "FXAA", "TAA", "SMAA" });

        }


        if (SettingsSceneValues.Instance.newAntiNone == true)
        {
            antiAliasingDropdown.value = (int)AntiAliasingMode.None;

        }

        if (SettingsSceneValues.Instance.newAntiFXAA == true)
        {
            antiAliasingDropdown.value = (int)AntiAliasingMode.FXAA;

        }
        
        if (SettingsSceneValues.Instance.newAntiTAA == true)
        {
            antiAliasingDropdown.value = (int)AntiAliasingMode.TAA;

        }

        if (SettingsSceneValues.Instance.newAntiSMAA == true)
        {
            antiAliasingDropdown.value = (int)AntiAliasingMode.SMAA;

        }
       
  

    }

    public void OnAntiDropdownValueChanged(int value)
    {
        AntiAliasingMode mode = (AntiAliasingMode)value;
        SetAntiAliasingMode(mode);

    }

    private void SetAntiAliasingMode(AntiAliasingMode mode)
    {
        switch (mode)
        {
            //call function depending on which mode selected
            case AntiAliasingMode.None:
                SettingsSceneValues.Instance.newAntiNone = true;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;

            case AntiAliasingMode.FXAA:
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = true;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;

            case AntiAliasingMode.TAA:
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = true;
                SettingsSceneValues.Instance.newAntiSMAA = false;
                break;

            case AntiAliasingMode.SMAA:
                SettingsSceneValues.Instance.newAntiNone = false;
                SettingsSceneValues.Instance.newAntiFXAA = false;
                SettingsSceneValues.Instance.newAntiTAA = false;
                SettingsSceneValues.Instance.newAntiSMAA = true;
                break;
        }
    }

    public void AudioEnable()
    {
        // Attempt to find the slider
        GameObject sliderObject = GameObject.Find("AudioSlider");
        GameObject audioObject = GameObject.Find("AudioManager");

        if (sliderObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
            volume = sliderObject.GetComponent<Slider>();

            if (volume != null)
            {
                // Add listener
                volume.onValueChanged.AddListener(OnVolumeChanged);
                volume.value = SettingsSceneValues.Instance.volumeNew;
            }
        
        }
   
    }
    public void OnVolumeChanged(float value)
    {
        SettingsSceneValues.Instance.volumeNew = value;

        audioManager.globalVolume = value;
        audioManager.soundEffectVolume = value;
        audioManager.musicVolume = value;


    }
}
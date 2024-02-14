using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private FPSCounter fpsCounter;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TextMeshProUGUI vsyncButtonText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private RawImage overlayImage;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] private RectTransform panel;

    private float originalAspect;  
    private float tempBrightnessValue;
    private bool tempFPSDisplayValue;

    private bool overlayVisible = false;
    private bool vsyncEnabled = true;
    private bool fpsDisplayEnabled = true;
    private bool applyButtonClicked = false;

    private string selectedResolution;
    private string tempSelectedResolution;

    private static SettingsManager _instance;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SettingsManager").AddComponent<SettingsManager>();
            }

            return _instance;
        }
    }

    #region Initialization and Singleton

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        originalAspect = panel.localScale.x / panel.localScale.y;
        // scaleSlider.onValueChanged.AddListener(SetPanelScale);
    }

    #endregion

    #region Unity Lifecycle Methods

    void Start()
    {
        LoadSettings();

        if (vsyncButton == null || vsyncButtonText == null || resolutionDropdown == null)
        {
            Debug.LogError("UI references are not assigned in the Unity Editor.");
            return;
        }

        PopulateResolutions();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        UpdateVSyncButtonText();
        ToggleFPSDisplay();

        overlayImage.gameObject.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    void Update()
    {
        if (fpsDisplayEnabled)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
        }
    }

    #endregion

    #region UI Interaction Methods

    public void OnToggleFPSButtonClicked()
    {
        fpsDisplayEnabled = !fpsDisplayEnabled;
        tempFPSDisplayValue = fpsDisplayEnabled; // Store the temporary value
        ToggleFPSDisplay();
        Debug.Log("FPS Display Setting Changed: " + fpsDisplayEnabled);
    }

    public void OnToggleVSyncButtonClicked()
    {
        vsyncEnabled = !vsyncEnabled;
        UpdateVSyncButtonText();
        Debug.Log("VSync Setting Changed: " + vsyncEnabled);
    }

    public void OnApplyButtonClicked()
    {
        SaveSettings();
        applyButtonClicked = true;

        // Ensure that the resolution dropdown has options
        if (resolutionDropdown.options.Count > 0)
        {
            // Get the currently selected option index
            int selectedIndex = resolutionDropdown.value;

            // Update selectedResolution using the index
            selectedResolution = resolutionDropdown.options[selectedIndex].text;

            // Apply the original resolution
            if (!string.IsNullOrEmpty(selectedResolution))
            {
                SetResolution(selectedResolution);
                Debug.Log("Settings Applied: VSync=" + vsyncEnabled + ", FPS Display=" + fpsDisplayEnabled + ", Resolution=" + selectedResolution);
            }
            else
            {
                Debug.LogError("Selected resolution is null or empty.");
            }

            // Apply the temporary resolution only if it differs from the original resolution
            if (!string.IsNullOrEmpty(tempSelectedResolution) && tempSelectedResolution != selectedResolution)
            {
                SetResolution(tempSelectedResolution);
                selectedResolution = tempSelectedResolution; // Update the selected resolution
            }
        }
        else
        {
            Debug.LogError("Resolution dropdown has no options.");
        }

        // Save the brightness value to PlayerPrefs
        
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Brightness Setting Changed: " + brightnessSlider.value);
    }


    public void OnBackButtonClicked()
    {
        if (!applyButtonClicked)
        {
            // The Apply button was not clicked, revert changes
            LoadSettings(true); // Update the UI with loaded settings
            UpdateVSyncButtonText();
            settingsPanel.SetActive(false);
            pauseMenu.SetActive(true);
            selectedResolution = null;
            tempSelectedResolution = null;

            // Revert FPS display value to the temporary value
            fpsDisplayEnabled = tempFPSDisplayValue;
            ToggleFPSDisplay();

            // Revert brightness value to the temporary value
            brightnessSlider.value = tempBrightnessValue;
            PlayerPrefs.SetFloat("Brightness", tempBrightnessValue);
            PlayerPrefs.Save();
            Debug.Log("Brightness Setting Reverted: " + tempBrightnessValue);

            Debug.Log("Original Settings Reverted");
        }
        else
        {
            // The Apply button was clicked, do not revert changes
            settingsPanel.SetActive(false);
            pauseMenu.SetActive(true);
            selectedResolution = null;
            tempSelectedResolution = null;
        }
        applyButtonClicked = false; // Reset the flag after processing
    }



    #endregion

    #region Settings Manipulation Methods

    private void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutions = new List<string>
        {
            "3840x2160", // 4k
            "2560x1440", // QHD
            "1920x1080", // Full HD
            "1366x768",
            "1280x1024",
            "1440x900",
            "1600x900",
            "1680x1050",
            "1280x800",
            "1024x768"
        };

        resolutionDropdown.AddOptions(resolutions);
    }

    private void OnResolutionChanged(int index)
    {
        string selectedResolution = resolutionDropdown.options[index].text;
        SetResolution(selectedResolution);
    }

    private void SetResolution(string resolution)
    {
        // Define possible separators
        char[] separators = { 'x', '×', 'X' };

        // Split the resolution using the possible separators
        string[] resolutionValues = resolution.Split(separators);

        if (resolutionValues != null && resolutionValues.Length == 2)
        {
            if (int.TryParse(resolutionValues[0], out int width) && int.TryParse(resolutionValues[1], out int height))
            {
                // Use the desired display index, 0 is the primary display
                int displayIndex = 0;

                // Set resolution for the entire screen
                Screen.SetResolution(width, height, Screen.fullScreen, displayIndex);
                Debug.Log("Resolution set to " + resolution);
            }
            else
            {
                Debug.LogError("Failed to parse resolution values. Invalid numeric format.");
            }
        }
        else
        {
            Debug.LogError("Invalid resolution format: " + resolution);
        }
    }

    private void OnBrightnessChanged(float value)
    {
        if (!overlayVisible)
        {
            overlayImage.gameObject.SetActive(true);
            overlayVisible = true;
        }

        float brightnessValue = Mathf.Lerp(0f, 0.01f, value);

        Color adjustedColor = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, brightnessValue);
        overlayImage.color = adjustedColor;

        // Store the temporary brightness value when the slider changes
        tempBrightnessValue = value;
    }

    private void OnGlobalVolumeChanged(float value)
    {
        audioManager.globalVolume = value;
    }

    private void OnSoundEffectVolumeChanged(float value)
    {
        audioManager.soundEffectVolume = value;
    }

    private void OnMusicVolumeChanged(float value)
    {
        audioManager.musicVolume = value;
    }

    private void ToggleFPSDisplay()
    {
        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(fpsDisplayEnabled);
        }
        else
        {
            Debug.LogError("TextMeshPro Text component not found for displaying FPS.");
        }
    }

    private void UpdateVSyncButtonText()
    {
        if (vsyncButtonText != null)
        {
            vsyncButtonText.text = "VSync: " + (vsyncEnabled ? "On" : "Off");
        }
        else
        {
            Debug.LogError("TextMeshPro Text component not found on the VSync Button.");
        }
    }

    #endregion

    #region Helper Methods


    // Recursive method to set the scale of the panel and its children
    // private void SetPanelScaleRecursive(Transform parent, float scaleValue)
    // {
    //     float newValx = scaleValue * originalAspect;
    //     float newValy = scaleValue;
    //
    //     RectTransform rectTransform = parent.GetComponent<RectTransform>();
    //     if (rectTransform != null)
    //     {
    //         rectTransform.localScale = new Vector3(newValx, newValy, 1f);
    //     }
    //
    // Recursively apply scaling to child objects
    //    foreach (Transform child in parent)
    //    {
    //        SetPanelScaleRecursive(child, scaleValue);
    //    }
    //
    //   originalSize = panel.localScale;
    // }

    // Call this method from SetPanelScale
    // private void SetPanelScale(float scaleValue)
    // {
    //      SetPanelScaleRecursive(panel, scaleValue);
    //}


    private void SaveSettings()
    {
        // Save all settings to PlayerPrefs
        PlayerPrefs.SetInt("VSync", vsyncEnabled ? 1 : 0);
        PlayerPrefs.SetInt("FPSDisplay", fpsDisplayEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("GlobalVolume", audioManager.globalVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", audioManager.soundEffectVolume);
        PlayerPrefs.SetFloat("MusicVolume", audioManager.musicVolume);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.SetFloat("PanelScale", scaleSlider.value);
        // Add more settings if needed
        PlayerPrefs.Save();
    }

    private void LoadSettings(bool toggleFPSCounter = true)
    {
        // Load all settings from PlayerPrefs
        if (PlayerPrefs.HasKey("VSync"))
        {
            vsyncEnabled = PlayerPrefs.GetInt("VSync") == 1;
        }

        if (PlayerPrefs.HasKey("FPSDisplay"))
        {
            fpsDisplayEnabled = PlayerPrefs.GetInt("FPSDisplay") == 1;

            if (toggleFPSCounter)
            {
                ToggleFPSDisplay();
            }
        }

        // Load other settings
        if (PlayerPrefs.HasKey("GlobalVolume"))
        {
            audioManager.globalVolume = PlayerPrefs.GetFloat("GlobalVolume");
        }

        if (PlayerPrefs.HasKey("SoundEffectVolume"))
        {
            audioManager.soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioManager.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }

        // Load brightness and panel scale
        if (PlayerPrefs.HasKey("Brightness"))
        {
            float brightnessValue = PlayerPrefs.GetFloat("Brightness");
            OnBrightnessChanged(brightnessValue); // Apply brightness
        }

        // if (PlayerPrefs.HasKey("PanelScale"))
        // {
        //     float scaleValue = PlayerPrefs.GetFloat("PanelScale");
        //     SetPanelScale(scaleValue); // Apply panel scale
        // }

        // Load more settings if needed
    }

    #endregion
}

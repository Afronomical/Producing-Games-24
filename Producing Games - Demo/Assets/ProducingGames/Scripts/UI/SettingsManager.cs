using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI vsyncButtonText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown maxFPSDropdown;
    [SerializeField] private RawImage overlayImage;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private RectTransform panel;
    [SerializeField] private FPSCounter fpsCounter;
    [SerializeField] private int defaultMaxFPS = 60; // defult Max FPS
    


    private List<int> availableMaxFPSOptions = new List<int> { 30, 60, 120, 144, 240 }; // Max FPS options // Customize as needed

    private int selectedMaxFPS;
    private int currentMaxFPS;
    private int originalMaxFPS;

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

        // Set the default max FPS
        Application.targetFrameRate = defaultMaxFPS;

        if (vsyncButton == null || vsyncButtonText == null || resolutionDropdown == null)
        {
            Debug.LogError("UI references are not assigned in the Unity Editor.");
            return;
        }

        PopulateResolutions();
        PopulateMaxFPSDropdown();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        maxFPSDropdown.onValueChanged.AddListener(OnMaxFPSChanged);

        UpdateVSyncButtonText();
        ToggleFPSDisplay();

        overlayImage.gameObject.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // Set the dropdown to show the current resolution
        SetDropdownToCurrentResolution();

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

        if (applyButtonClicked)
        {
            ApplyMaxFPSSetting();
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

            // Revert the resolution value to the one stored in PlayerPrefs
            tempSelectedResolution = PlayerPrefs.GetString("Resolution", selectedResolution);

            // Update the dropdown to show the current resolution
            SetDropdownToCurrentResolution();

            // Update the dropdown to show the reverted resolution
            int index = resolutionDropdown.options.FindIndex(option => option.text == tempSelectedResolution);
            if (index != -1)
            {
                resolutionDropdown.value = index;
                resolutionDropdown.RefreshShownValue();
            }

            // Revert FPS display value to the temporary value
            fpsDisplayEnabled = tempFPSDisplayValue;
            ToggleFPSDisplay();

            // Revert brightness value to the temporary value
            brightnessSlider.value = tempBrightnessValue;
            PlayerPrefs.SetFloat("Brightness", tempBrightnessValue);
            PlayerPrefs.Save();
            Debug.Log("Brightness Setting Reverted: " + tempBrightnessValue);

            // Revert the max FPS value to the original value
            selectedMaxFPS = originalMaxFPS;

            // Update the dropdown to show the current max FPS
            SetDropdownToCurrentMaxFPS();

            ApplyMaxFPSSetting();  // Apply the reverted max FPS setting

            // Update other settings as needed...

            Debug.Log("Original Settings Reverted");
        }
        else
        {
            // The Apply button was clicked, do not revert changes
            settingsPanel.SetActive(false);
            pauseMenu.SetActive(true);
            selectedResolution = null;
        }
        applyButtonClicked = false; // Reset the flag after processing
    }
    



    private void SetDropdownToCurrentResolution()
    {
        Resolution currentResolution = Screen.currentResolution;
        string currentResolutionString = $"{currentResolution.width}x{currentResolution.height}";

        int index = resolutionDropdown.options.FindIndex(option => option.text == currentResolutionString);
        if (index != -1)
        {
            resolutionDropdown.value = index;
            resolutionDropdown.RefreshShownValue();
        }
    }

    #endregion

    #region Settings Manipulation Methods


    // RESOLUTION
    private void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutions = new List<string>();

        foreach (var resolution in Screen.resolutions)
        {
            resolutions.Add($"{resolution.width}x{resolution.height}");
        }

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


    // MAX FPS
    private void PopulateMaxFPSDropdown()
    {
        maxFPSDropdown.ClearOptions();
        List<string> maxFPSOptions = new List<string>();

        foreach (int option in availableMaxFPSOptions)
        {
            maxFPSOptions.Add(option.ToString());
        }

        maxFPSDropdown.AddOptions(maxFPSOptions);
    }

    private void OnMaxFPSChanged(int index)
    {
        selectedMaxFPS = availableMaxFPSOptions[index];
    }

    private void ApplyMaxFPSSetting()
    {
        // Apply the selected max FPS setting
        Application.targetFrameRate = selectedMaxFPS;
        Debug.Log("Max FPS set to: " + selectedMaxFPS);
    }

    private void SetDropdownToCurrentMaxFPS()
    {
        int index = availableMaxFPSOptions.IndexOf(selectedMaxFPS);

        if (index != -1)
        {
            maxFPSDropdown.value = index;
            maxFPSDropdown.RefreshShownValue();
        }
    }


    // Brightness slider
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

        // Load VSync setting
        if (PlayerPrefs.HasKey("VSync"))
        {
            vsyncEnabled = PlayerPrefs.GetInt("VSync") == 1;
        }

        // Load FPS Display setting
        if (PlayerPrefs.HasKey("FPSDisplay"))
        {
            fpsDisplayEnabled = PlayerPrefs.GetInt("FPSDisplay") == 1;

            // Toggle FPS Display if needed
            if (toggleFPSCounter)
            {
                ToggleFPSDisplay();
            }
        }

        // Load other audio settings
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

        // Load brightness and apply it
        if (PlayerPrefs.HasKey("Brightness"))
        {
            float brightnessValue = PlayerPrefs.GetFloat("Brightness");
            OnBrightnessChanged(brightnessValue); // Apply brightness
        }

        // Load the resolution setting and update the UI
        if (PlayerPrefs.HasKey("Resolution"))
        {
            tempSelectedResolution = PlayerPrefs.GetString("Resolution");
            int index = resolutionDropdown.options.FindIndex(option => option.text == tempSelectedResolution);

            if (index != -1)
            {
                resolutionDropdown.value = index;
                resolutionDropdown.RefreshShownValue();
            }
        }

        // Load the max FPS setting and update the UI
        if (PlayerPrefs.HasKey("MaxFPS"))
        {
            int loadedMaxFPS = PlayerPrefs.GetInt("MaxFPS", defaultMaxFPS);

            // Ensure that the loaded max FPS is within the available options
            if (availableMaxFPSOptions.Contains(loadedMaxFPS))
            {
                selectedMaxFPS = loadedMaxFPS;
                currentMaxFPS = loadedMaxFPS;  // Store the currently selected max FPS
                originalMaxFPS = loadedMaxFPS; // Store the original max FPS
            }
            else
            {
                // If the loaded value is not in the available options, use the default
                selectedMaxFPS = defaultMaxFPS;
                currentMaxFPS = defaultMaxFPS;
                originalMaxFPS = defaultMaxFPS;
            }

            // Update the dropdown to show the current max FPS
            SetDropdownToCurrentMaxFPS();
        }
        else
        {
            // If the max FPS setting is not found in PlayerPrefs, use the default
            selectedMaxFPS = defaultMaxFPS;
            currentMaxFPS = defaultMaxFPS;
            originalMaxFPS = defaultMaxFPS;
        }

        // Load more settings if needed
    }
    #endregion
}
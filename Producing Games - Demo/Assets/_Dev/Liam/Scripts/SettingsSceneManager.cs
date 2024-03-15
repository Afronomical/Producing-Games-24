using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TextMeshProUGUI vsyncButtonText;

    private static bool vsyncEnabled = true; // Static variable to hold VSync state

    void Start()
    {
        // Initialize UI elements
        InitializeUI();
        Debug.Log("VSync is " + (vsyncEnabled ? "enabled." : "disabled."));
    }

    void InitializeUI()
    {
        // Initialize Display Mode Dropdown
        InitializeDisplayModeDropdown();

        // Initialize Resolution Dropdown
        InitializeResolutionDropdown();

        // Initialize VSync button
        UpdateVSyncButtonText();
        vsyncButton.onClick.AddListener(ToggleVSync);
    }

    // Method to toggle VSync on/off
    void ToggleVSync()
    {
        vsyncEnabled = !vsyncEnabled;
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        UpdateVSyncButtonText();
        Debug.Log("VSync is " + (vsyncEnabled ? "enabled." : "disabled."));
    }

    // Method to update VSync button text
    void UpdateVSyncButtonText()
    {
        vsyncButtonText.text = vsyncEnabled ? "VSync: On" : "VSync: Off";
    }

    // Method to initialize the Display Mode Dropdown
    void InitializeDisplayModeDropdown()
    {
        // Clear any existing options
        displayModeDropdown.ClearOptions();

        // Add new options
        displayModeDropdown.AddOptions(new List<string> { "Full Screen", "Windowed", "Windowed Full Screen" });

        // Add listener to the dropdown value changed event
        displayModeDropdown.onValueChanged.AddListener(delegate {
            DisplayModeDropdownValueChanged(displayModeDropdown);
        });
    }

    // Method called when display mode dropdown value changes
    void DisplayModeDropdownValueChanged(TMP_Dropdown change)
    {
        switch (displayModeDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    // Method to initialize the Resolution Dropdown
    void InitializeResolutionDropdown()
    {
        // Clear any existing options
        resolutionDropdown.ClearOptions();

        // Define your custom resolutions
        List<string> customResolutions = new List<string>
        {
            "1920x1080", "1280x720", "2560x1440", "3840x2160", "1680x1050",
            "1600x900", "1366x768", "1280x800", "1024x768", "800x600",
            "3440x1440", "2560x1080", "2560x1600", "1920x1200","3840x2160"

        };

        // Filter supported resolutions from the custom list
        List<string> supportedResolutions = new List<string>();
        foreach (string resolution in customResolutions)
        {
            string[] resolutionParts = resolution.Split('x');
            int width = int.Parse(resolutionParts[0]);
            int height = int.Parse(resolutionParts[1]);

            // Check if the custom resolution is supported
            if (IsResolutionSupported(width, height))
            {
                supportedResolutions.Add(resolution);
            }
        }

        // Add supported resolutions to the dropdown options
        resolutionDropdown.AddOptions(supportedResolutions);

        // Add listener to the dropdown value changed event
        resolutionDropdown.onValueChanged.AddListener(delegate {
            ResolutionDropdownValueChanged(resolutionDropdown);
        });
    }

    // Method to check if a resolution is supported
    bool IsResolutionSupported(int width, int height)
    {
        // Get available resolutions
        Resolution[] availableResolutions = Screen.resolutions;

        // Check if the resolution matches any available resolutions
        foreach (Resolution resolution in availableResolutions)
        {
            if (width == resolution.width && height == resolution.height)
            {
                return true;
            }
        }
        return false;
    }

    // Method called when resolution dropdown value changes
    void ResolutionDropdownValueChanged(TMP_Dropdown change)
    {
        // Parse the selected resolution from the dropdown text
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolutionParts = selectedResolution.Split('x');
        int width = int.Parse(resolutionParts[0]);
        int height = int.Parse(resolutionParts[1]);

        // Set the screen resolution
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }
}

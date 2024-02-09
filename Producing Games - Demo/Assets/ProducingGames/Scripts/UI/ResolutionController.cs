using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        PopulateResolutions();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutions = new List<string>();

        // Add your desired resolutions here
        resolutions.Add("3840×2160"); // 4k
        resolutions.Add("2560×1440"); // QHD
        resolutions.Add("1920x1080"); // Full HD
        resolutions.Add("1366×768");
        resolutions.Add("1280×1024");
        resolutions.Add("1440×900");
        resolutions.Add("1600×900");
        resolutions.Add("1680×1050");
        resolutions.Add("1280×800");
        resolutions.Add("1024×768");

        // Add more resolutions if needed

        resolutionDropdown.AddOptions(resolutions);
    }

    private void OnResolutionChanged(int index)
    {
        string selectedResolution = resolutionDropdown.options[index].text;
        SetResolution(selectedResolution);
    }

    private void SetResolution(string resolution)
    {
        {
            // Define possible separators
            char[] separators = { 'x', '×', 'X' };

            // Split the resolution using the possible separators
            string[] resolutionValues = resolution.Split(separators);

            if (resolutionValues.Length == 2)
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
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsManager : MonoBehaviour
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
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private RectTransform panel;
    [SerializeField] private FPSCounter fpsCounter;
    [SerializeField] private int defaultMaxFPS = 60;

    [Header("FOV")]
    public Slider fovSlider;
    public Camera mainCamera;
    public Button defaultFov;

    [Header("Display Mode")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    private FullScreenMode tempDisplayMode;

    private List<int> availableMaxFPSOptions = new List<int> { 30, 60, 120, 144, 240 };
    private List<string> displayModeOptions = new List<string> { "Windowed", "Fullscreen", "Windowed Fullscreen" };

    private int selectedMaxFPS;
    private int originalMaxFPS;

    private float originalAspect;
    private float tempBrightnessValue;

    private bool tempFPSDisplayValue;
    private bool overlayVisible = false;
    private bool vsyncEnabled;
    private bool fpsDisplayEnabled = true;

    private string selectedResolution;
    private string tempSelectedResolution;

    // Default values
    [SerializeField] private Button resetButton;
    private bool defaultVSyncEnabled = true;
    private bool defaultFPSDisplayEnabled = true;
    private string defaultResolution = "1920x1080";
    private int defaultFPS = 60;
    private float defaultFOV = 60f;
    private float defaultBrightness = 0f;

    private static VideoSettingsManager _instance;

    public static VideoSettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("VideoSettingsManager").AddComponent<VideoSettingsManager>();
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

        overlayImage.gameObject.SetActive(false);

        originalAspect = panel.localScale.x / panel.localScale.y;
    }

    void Start()
    {


        Debug.Log("VSync is " + (vsyncEnabled ? "enabled." : "disabled."));

        Application.targetFrameRate = defaultMaxFPS;

        if (vsyncButton == null || vsyncButtonText == null || resolutionDropdown == null)
        {
            Debug.LogError("UI references are not assigned in the Unity Editor.");
            return;
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetVideoSettingsToDefaults);
        }
        else
        {
            Debug.LogError("Reset button reference not assigned in the Unity Editor.");
        }

        PopulateResolutions();
        PopulateMaxFPSDropdown();
        PopulateDisplayModeDropdown();
        // Populate UI elements with default values on start
        //SetDefaultValues();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        maxFPSDropdown.onValueChanged.AddListener(OnMaxFPSChanged);
        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);

        fovSlider.onValueChanged.AddListener(OnFOVChanged); // Add listener for FOV change

        UpdateVSyncButtonText();
        ToggleFPSDisplay();
        overlayVisible = false;

        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        SetDropdownToCurrentResolution();
        SetDropdownToCurrentMaxFPS();
        SetDropdownToCurrentDisplayMode();

        mainCamera.fieldOfView = SettingsSceneValues.Instance.FovNew;
        fovSlider.value = SettingsSceneValues.Instance.FovNew;
    }

    void Update()
    {
        if (fpsDisplayEnabled)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
        }

        UpdateVSyncButton();
    }

    public void OnToggleFPSButtonClicked()
    {
        fpsDisplayEnabled = !fpsDisplayEnabled;
        tempFPSDisplayValue = fpsDisplayEnabled;
        ToggleFPSDisplay();
        Debug.Log("FPS Display Setting Changed: " + fpsDisplayEnabled);
    }

    public void OnToggleVSyncButtonClicked()
    {
        vsyncEnabled = !vsyncEnabled;
        ApplyVSyncSetting();
        UpdateVSyncButtonText();
        Debug.Log("VSync Setting Changed: " + vsyncEnabled);
    }

    public void UpdateVSyncButton()
    {
        if (vsyncEnabled)
        {
            vsyncButtonText.text = "VSync: On";
        }
        else
        {
            vsyncButtonText.text = "VSync: Off";
        }
    }

    private void ApplyVSyncSetting()
    {
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        Debug.Log("VSync Setting Applied: " + (vsyncEnabled ? "On" : "Off"));
    }

    private void OnFOVChanged(float value)
    {
        mainCamera.fieldOfView = value;
        SettingsSceneValues.Instance.FovNew = value;
        // Add additional logic related to FOV setting
        Debug.Log("FOV Setting Changed: " + value);
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

        tempBrightnessValue = value;
        Debug.Log("Brightness Setting Changed: " + value);
    }

    private void OnMaxFPSChanged(int index)
    {
        selectedMaxFPS = availableMaxFPSOptions[index];
        ApplyMaxFPSSetting();
        Debug.Log("Max FPS Setting Changed: " + selectedMaxFPS);
    }

    private void OnResolutionChanged(int index)
    {
        tempSelectedResolution = resolutionDropdown.options[index].text;
        SetResolution(tempSelectedResolution);
        Debug.Log("Resolution Setting Changed: " + tempSelectedResolution);
    }

    private void OnDisplayModeChanged(int index)
    {
        string selectedDisplayMode = displayModeOptions[index];
        SetDisplayMode(selectedDisplayMode);
        Debug.Log("Display Mode Setting Changed: " + selectedDisplayMode);
    }

    private void SetResolution(string resolution)
    {
        char[] separators = { 'x', 'X', 'X' };
        string[] resolutionValues = resolution.Split(separators);

        if (resolutionValues != null && resolutionValues.Length == 2)
        {
            if (int.TryParse(resolutionValues[0], out int width) && int.TryParse(resolutionValues[1], out int height))
            {
                int displayIndex = 0;
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

    private void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> preferredResolutions = new List<string>
        {
            "1920x1080", "1280x720", "2560x1440", "3840x2160", "1680x1050",
            "1600x900", "1366x768", "1280x800", "1024x768", "800x600",
            "3440x1440", "2560x1080", "2560x1600", "1920x1200","3840x2160"

        };

        List<string> supportedResolutions = new List<string>();

        foreach (var resolution in preferredResolutions)
        {
            int width, height;
            ParseResolution(resolution, out width, out height);

            // Check if the current system supports the resolution.
            if (IsResolutionSupported(width, height))
            {
                supportedResolutions.Add(resolution);
            }
        }

        resolutionDropdown.AddOptions(supportedResolutions);
    }

    private void ParseResolution(string resolution, out int width, out int height)
    {
        string[] parts = resolution.Split('x');
        width = int.Parse(parts[0]);
        height = int.Parse(parts[1]);
    }

    private bool IsResolutionSupported(int width, int height)
    {
        foreach (var systemResolution in Screen.resolutions)
        {
            if (systemResolution.width == width && systemResolution.height == height)
            {
                return true;
            }
        }
        return false;
    }

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

    private void ApplyMaxFPSSetting()
    {
        Application.targetFrameRate = selectedMaxFPS;
        Debug.Log("Max FPS set to: " + selectedMaxFPS);
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

    private void SetDropdownToCurrentMaxFPS()
    {
        int index = availableMaxFPSOptions.IndexOf(selectedMaxFPS);

        if (index != -1)
        {
            maxFPSDropdown.value = index;
            maxFPSDropdown.RefreshShownValue();
        }
    }

    private void PopulateDisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(displayModeOptions);
    }

    private void SetDisplayMode(string displayMode)
    {
        switch (displayMode)
        {
            case "Windowed":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case "Fullscreen":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case "Windowed Fullscreen":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Debug.LogError("Invalid display mode: " + displayMode);
                break;
        }

        tempDisplayMode = Screen.fullScreenMode;
    }

    private void SetDropdownToCurrentDisplayMode()
    {
        FullScreenMode currentDisplayMode = Screen.fullScreenMode;
        string currentDisplayModeString = GetDisplayModeString(currentDisplayMode);

        int index = displayModeOptions.IndexOf(currentDisplayModeString);
        if (index != -1)
        {
            displayModeDropdown.value = index;
            displayModeDropdown.RefreshShownValue();
        }
    }

    private string GetDisplayModeString(FullScreenMode displayMode)
    {
        switch (displayMode)
        {
            case FullScreenMode.Windowed:
                return "Windowed";
            case FullScreenMode.FullScreenWindow:
                return "Fullscreen";
            case FullScreenMode.ExclusiveFullScreen:
                return "Windowed Fullscreen";
            default:
                return "Unknown";
        }
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

    public void ResetVideoSettingsToDefaults()
    {
        SetDefaultValues();
        Debug.Log("Settings Reset to Default Values");
    }

    private void SetDefaultValues()
    {
        // Set default values for video settings
        vsyncEnabled = defaultVSyncEnabled;
        ApplyVSyncSetting();
        ToggleFPSDisplay();
        SetResolution(defaultResolution);
        selectedMaxFPS = defaultFPS;
        ApplyMaxFPSSetting();
        fovSlider.value = defaultFOV;
        OnFOVChanged(defaultFOV);
        brightnessSlider.value = defaultBrightness;
        OnBrightnessChanged(defaultBrightness);

        // Update UI elements
        SetDropdownToCurrentResolution();
        SetDropdownToCurrentMaxFPS();
        UpdateVSyncButtonText();

        // Update slider visuals
       // fovSlider.GetComponentInChildren<TextMeshProUGUI>().text = defaultFOV.ToString(); // Update FOV slider text
        //brightnessSlider.GetComponentInChildren<TextMeshProUGUI>().text = defaultBrightness.ToString(); // Update brightness slider text

        Debug.Log("Settings Reset to Default Values:");
        Debug.Log("VSync: " + (vsyncEnabled ? "On" : "Off"));
        Debug.Log("Resolution: " + defaultResolution);
        Debug.Log("Max FPS: " + defaultFPS);
        Debug.Log("FOV: " + defaultFOV);
        Debug.Log("Brightness: " + defaultBrightness);
    }
}
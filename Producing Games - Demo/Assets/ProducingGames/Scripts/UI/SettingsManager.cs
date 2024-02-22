using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject checkBox;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI vsyncButtonText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown maxFPSDropdown;
    [SerializeField] private RawImage overlayImage;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private RectTransform panel;
    [SerializeField] private FPSCounter fpsCounter;
    [SerializeField] private int defaultMaxFPS = 60;

    [Header("Inventory Elements")]
    public RectTransform inventoryPanel;
    public float increaseInvEffectSpeed = 3f;
    public float decreaseInvEffectSpeed = 3f;
    public float invEffectWaitDuration = 1.5f;
    public Slider inventoryScaleSlider;
    [HideInInspector] public Vector3 originalInventorySize;
    [HideInInspector] public bool isInventoryIncreasing = false;
    [HideInInspector] public CanvasGroup inventoryPanelCanvasGroup;
    [HideInInspector] public float originalInventoryAspect;

    [Header("FOV")]
    public Slider fovSlider;
    public Camera mainCamera;
    public Button defaultFov;

    [Header("Display Mode")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    private bool applyDisplayModeClicked = false;
    private FullScreenMode tempDisplayMode;

    private List<int> availableMaxFPSOptions = new List<int> { 30, 60, 120, 144, 240 };
    private List<string> displayModeOptions = new List<string> { "Windowed", "Fullscreen", "Windowed Fullscreen" };

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

    private class TemporarySettings
    {
        public bool VSyncEnabled;
        public bool FPSDisplayEnabled;
        public float GlobalVolume;
        public float SoundEffectVolume;
        public float MusicVolume;
        public float Brightness;
        public float InventorySize;
        public int MaxFPS;
        public FullScreenMode DisplayMode;
    }

    private TemporarySettings tempSettings = new TemporarySettings();

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
        inventoryScaleSlider.onValueChanged.AddListener(OninventoryPanelScaleChanged);
    }

    void Start()
    {
        Application.targetFrameRate = defaultMaxFPS;

        if (vsyncButton == null || vsyncButtonText == null || resolutionDropdown == null)
        {
            Debug.LogError("UI references are not assigned in the Unity Editor.");
            return;
        }

        PopulateResolutions();
        PopulateMaxFPSDropdown();
        PopulateDisplayModeDropdown();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        maxFPSDropdown.onValueChanged.AddListener(OnMaxFPSChanged);
        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);

        UpdateVSyncButtonText();
        ToggleFPSDisplay();
        overlayVisible = false;

        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        SetDropdownToCurrentResolution();
        SetDropdownToCurrentMaxFPS();
        SetDropdownToCurrentDisplayMode();
    }

    void Update()
    {
        if (fpsDisplayEnabled)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
        }
    }

    public void OninventoryPanelScaleChanged(float value)
    {
        tempSettings.InventorySize = value;
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
        UpdateVSyncButtonText();
        Debug.Log("VSync Setting Changed: " + vsyncEnabled);
    }

    public void OnApplyButtonClicked()
    {
        SaveTemporarySettings();
        applyButtonClicked = true;

        if (resolutionDropdown.options.Count > 0)
        {
            int selectedIndex = resolutionDropdown.value;
            selectedResolution = resolutionDropdown.options[selectedIndex].text;

            if (!string.IsNullOrEmpty(selectedResolution))
            {
                SetResolution(selectedResolution);
                Debug.Log("Settings Applied: VSync=" + tempSettings.VSyncEnabled + ", FPS Display=" + tempSettings.FPSDisplayEnabled + ", Resolution=" + selectedResolution);
            }
            else
            {
                Debug.LogError("Selected resolution is null or empty.");
            }

            if (!string.IsNullOrEmpty(tempSelectedResolution) && tempSelectedResolution != selectedResolution)
            {
                SetResolution(tempSelectedResolution);
                selectedResolution = tempSelectedResolution;
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

        Debug.Log("Brightness Setting Changed: " + brightnessSlider.value);
    }

    public void OnBackButtonClicked()
    {
        if (!applyButtonClicked)
        {
            LoadTemporarySettings();
            UpdateVSyncButtonText();
            settingsPanel.SetActive(false);
            pauseMenu.SetActive(true);
            selectedResolution = null;

            tempSelectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;

            SetDropdownToCurrentResolution();

            int index = resolutionDropdown.options.FindIndex(option => option.text == tempSelectedResolution);
            if (index != -1)
            {
                resolutionDropdown.value = index;
                resolutionDropdown.RefreshShownValue();
            }

            fpsDisplayEnabled = tempFPSDisplayValue;
            ToggleFPSDisplay();

            brightnessSlider.value = tempBrightnessValue;
            Debug.Log("Brightness Setting Reverted: " + tempBrightnessValue);

            inventoryScaleSlider.value = tempSettings.InventorySize;
            Debug.Log("Inventory scale Setting Reverted: " + tempSettings.InventorySize);

            selectedMaxFPS = originalMaxFPS;

            SetDropdownToCurrentMaxFPS();

            ApplyMaxFPSSetting();

            Debug.Log("Original Settings Reverted");
        }
        else
        {
            settingsPanel.SetActive(false);
            pauseMenu.SetActive(true);
            selectedResolution = null;
        }
        applyButtonClicked = false;
    }

    public void OnBackCheck()
    {
        checkBox.SetActive(true);
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
        char[] separators = { 'x', '�', 'X' };
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

    private void PopulateDisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(displayModeOptions);
    }

    private void OnDisplayModeChanged(int index)
    {
        string selectedDisplayMode = displayModeOptions[index];
        SetDisplayMode(selectedDisplayMode);
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

    private void SaveTemporarySettings()
    {
        tempSettings.VSyncEnabled = vsyncEnabled;
        tempSettings.FPSDisplayEnabled = fpsDisplayEnabled;
        tempSettings.GlobalVolume = audioManager.globalVolume;
        tempSettings.SoundEffectVolume = audioManager.soundEffectVolume;
        tempSettings.MusicVolume = audioManager.musicVolume;
        tempSettings.Brightness = tempBrightnessValue;
        tempSettings.InventorySize = inventoryScaleSlider.value;
        tempSettings.MaxFPS = selectedMaxFPS;
        tempSettings.DisplayMode = tempDisplayMode;
    }

    private void LoadTemporarySettings()
    {
        vsyncEnabled = tempSettings.VSyncEnabled;
        fpsDisplayEnabled = tempSettings.FPSDisplayEnabled;
        audioManager.globalVolume = tempSettings.GlobalVolume;
        audioManager.soundEffectVolume = tempSettings.SoundEffectVolume;
        audioManager.musicVolume = tempSettings.MusicVolume;
        tempBrightnessValue = tempSettings.Brightness;
        inventoryScaleSlider.value = tempSettings.InventorySize;
        selectedMaxFPS = tempSettings.MaxFPS;
        tempDisplayMode = tempSettings.DisplayMode;
        applyDisplayModeClicked = true;
    }
}


//private void ScaleUIElements(Transform parent)
//{
//    foreach (Transform child in parent)
//    {
//        // Check if the child has a Button or Slider component
//        Button button = child.GetComponent<Button>();
//        Slider slider = child.GetComponent<Slider>();

//        // Scale the UI elements based on the scaleSlider value
//        if (button != null)
//        {
//           ScaleButton(button);
//        }

//        if (slider != null)
//        {
//            ScaleSlider(slider);
//        }

//        // Recursively scale UI elements for child objects
//        ScaleUIElements(child);
//    }
//}

//private void ScaleUIElements()
//{
//    // Check if a panel is assigned
//    if (panel != null)
//    {
//        // Start the recursive scaling process from the panel
//        ScaleUIElements(panel);
//    }
//    else
//    {
//        Debug.LogError("Panel is not assigned. Cannot scale UI elements.");
//    }
//}

//private void ScaleButton(Button button)
//{
// Check if the button has a RectTransform
//    RectTransform rectTransform = button.GetComponent<RectTransform>();
//    if (rectTransform != null)
//    {
//        // Scale the button based on the scaleSlider value
//        float scaleValue = scaleSlider.value;
//        rectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);
//    }
//    else
//    {
//        Debug.LogError("Button does not have a RectTransform. Cannot scale.");
//    }
//}

//private void ScaleSlider(Slider slider)
//{
//    // Check if the slider has a RectTransform
//    RectTransform rectTransform = slider.GetComponent<RectTransform>();
//    if (rectTransform != null)
//    {
//        // Scale the slider based on the scaleSlider value
//        float scaleValue = scaleSlider.value;
//        rectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);
//    }
//    else
//    {
//        Debug.LogError("Slider does not have a RectTransform. Cannot scale.");
//    }
//}


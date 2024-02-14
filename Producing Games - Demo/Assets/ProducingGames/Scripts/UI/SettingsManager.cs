using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;

    public static SettingsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingsManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("SettingsManager");
                    instance = obj.AddComponent<SettingsManager>();
                }
            }

            return instance;
        }
    }

    private bool vsyncEnabled = true;
    private bool fpsDisplayEnabled = true;

    public GameObject settingsPanel;
    public GameObject pauseMenu;
    public FPSCounter FpsCounter;
    public Button vsyncButton;
    public TextMeshProUGUI vsyncButtonText;
    public Slider brightnessSlider;
    public RawImage overlayImage;
    public Color overlayColor = Color.black; 
    public Slider globalVolumeSlider;
    public Slider soundEffectVolumeSlider;
    public Slider musicVolumeSlider;
    public AudioManager audioManager;
    public Slider scaleSlider;
    public RectTransform panel; 
    private Vector3 originalSize;
    private float originalAspect;



    private bool overlayVisible = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        originalAspect = panel.localScale.x / panel.localScale.y;
        scaleSlider.onValueChanged.AddListener(SetPanelScale);
    }

    void Start()
    {
        overlayImage.gameObject.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        
        
        originalSize = panel.localScale;
       
        LoadSettings();
        

        

        if (vsyncButton == null || vsyncButtonText == null)
        {
            Debug.LogError("VSync Button or TextMeshPro Text reference is not assigned in the Unity Editor.");
            return;
        }

        UpdateVSyncButtonText();
    }

    void Update()
    {
        // Your other code here
    }

    private void SetPanelScaleRecursive(Transform parent, float scaleValue)
    {
        float newValx = scaleValue * originalAspect;
        float newValy = scaleValue;

        RectTransform rectTransform = parent.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Check if the current object is a Button or Slider before applying scaling
            if (parent.GetComponent<Button>())
            {
                rectTransform.localScale = new Vector3(newValx, newValy, 1f);
            }
            else if (parent.GetComponent<Slider>())
            {
                // Set slider to default size straight away
                rectTransform.localScale = new Vector3(newValx, newValy, 1f);
            }
        }

        // Recursively apply scaling to child objects
        foreach (Transform child in parent)
        {
            SetPanelScaleRecursive(child, scaleValue);
        }

        originalSize = panel.localScale;
    }

 // Call this method from SetPanelScale
    private void SetPanelScale(float scaleValue)
    {
        SetPanelScaleRecursive(panel, scaleValue);
    }

    private void OnBrightnessChanged(float value)
    {
        
        if (!overlayVisible)
        {
            overlayImage.gameObject.SetActive(true);
            overlayVisible = true;
        }

       
        float brightnessValue = Mathf.Lerp(0f, 0.01f, value);

      
        Color adjustedColor = new Color(overlayColor.r, overlayColor.g, overlayColor.b, brightnessValue);
        overlayImage.color = adjustedColor;
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

    public void OnToggleFPSButtonClicked()
    {
        fpsDisplayEnabled = !fpsDisplayEnabled;
        FpsCounter.ToggleFPSDisplay();
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
        Debug.Log("Settings Applied: VSync=" + vsyncEnabled + ", FPS Display=" + fpsDisplayEnabled);
    }

    public void OnBackButtonClicked()
    {
        settingsPanel.SetActive(false);
        pauseMenu.SetActive(true);
        LoadSettings(); // Reload the settings from PlayerPrefs
        UpdateVSyncButtonText();
        Debug.Log("Original Settings Reverted");
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

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("VSync", vsyncEnabled ? 1 : 0);
        PlayerPrefs.SetInt("FPSDisplay", fpsDisplayEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VSync"))
        {
            vsyncEnabled = PlayerPrefs.GetInt("VSync") == 1;
        }

        if (PlayerPrefs.HasKey("FPSDisplay"))
        {
            fpsDisplayEnabled = PlayerPrefs.GetInt("FPSDisplay") == 1;
            FpsCounter.ToggleFPSDisplay();
        }
    }
}

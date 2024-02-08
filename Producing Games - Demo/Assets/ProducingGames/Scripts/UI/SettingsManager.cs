using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
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

using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private AudioManager audioManager;

    private static AudioSettingsManager _instance;

    public static AudioSettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("AudioSettingsManager").AddComponent<AudioSettingsManager>();
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
        if (globalVolumeSlider == null || soundEffectVolumeSlider == null || musicVolumeSlider == null || audioManager == null)
        {
            Debug.LogError("UI references or AudioManager are not assigned in the Unity Editor.");
            return;
        }

        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void OnGlobalVolumeChanged(float value)
    {
        audioManager.globalVolume = value;
        // Add additional logic related to global volume setting
    }

    private void OnSoundEffectVolumeChanged(float value)
    {
        audioManager.soundEffectVolume = value;
        // Add additional logic related to sound effect volume setting
    }

    private void OnMusicVolumeChanged(float value)
    {
        audioManager.musicVolume = value;
        // Add additional logic related to music volume setting
    }
}


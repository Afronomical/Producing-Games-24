using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccessibilitySettingsManager : MonoBehaviour
{
    [Header("Accessibility Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider inventoryScaleSlider;

    [HideInInspector] public bool isInventoryIncreasing = false;
    [HideInInspector] public CanvasGroup inventoryPanelCanvasGroup;
    [HideInInspector] public float originalInventoryAspect;

    public float increaseEffectSpeed = 3f;
    public float decreaseEffectSpeed = 3f;
    public float effectWaitDuration = 1.5f;
    public float effectScaleMultiplier = 1.2f;

    private static AccessibilitySettingsManager _instance;

    public static AccessibilitySettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("AccessibilitySettingsManager").AddComponent<AccessibilitySettingsManager>();
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
        if (inventoryScaleSlider == null)
        {
            Debug.LogError("UI reference for inventory scale slider is not assigned in the Unity Editor.");
            return;
        }

        inventoryScaleSlider.onValueChanged.AddListener(OnInventoryPanelScaleChanged);
        originalInventoryAspect = inventoryScaleSlider.value;
    }

    public void OnInventoryPanelScaleChanged(float value)
    {
        // Add accessibility settings related functionality here
    }
}


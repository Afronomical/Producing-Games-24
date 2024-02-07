using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Slider brightnessSlider;
    public RawImage overlayImage;
    public Color overlayColor = Color.black; 

    private bool overlayVisible = false;

    private void Start()
    {
        
    
        overlayImage.gameObject.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    private void OnBrightnessChanged(float value)
    {
        
        ShowOverlay();

       
        float brightnessValue = Mathf.Lerp(0f, 0.1f, value);

      
        Color adjustedColor = new Color(overlayColor.r, overlayColor.g, overlayColor.b, brightnessValue);
        overlayImage.color = adjustedColor;
    }

    private void ShowOverlay()
    {
      
        if (!overlayVisible)
        {
            overlayImage.gameObject.SetActive(true);
            overlayVisible = true;
        }
    }
}
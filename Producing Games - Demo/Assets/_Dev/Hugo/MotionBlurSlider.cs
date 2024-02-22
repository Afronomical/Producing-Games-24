using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; 
using UnityEngine.UI;

public class MotionBlurController : MonoBehaviour
{
    public Volume volume;
    public Slider motionBlurSlider;

    private MotionBlur motionBlurVolume; 

    private void Start()
    {
        if (volume.profile.TryGet(out motionBlurVolume))
        {
            UpdateMotionBlurIntensity();
        }

        motionBlurSlider.onValueChanged.AddListener(OnMotionBlurSliderChanged);
    }

    private void OnMotionBlurSliderChanged(float value)
    {
        if (motionBlurVolume != null)
        {
            //update the motion blur setting in volume with value from slider
            motionBlurVolume.intensity.value = value;
        }
    }

    

    //set slider value to motionblur intensity when called in start
    private void UpdateMotionBlurIntensity()
    {
        if (motionBlurVolume != null)
        {
            motionBlurSlider.value = motionBlurVolume.intensity.value;
        }
    }
}
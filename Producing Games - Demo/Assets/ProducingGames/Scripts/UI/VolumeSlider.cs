using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider globalVolumeSlider;
    public Slider soundEffectVolumeSlider;
    public Slider musicVolumeSlider;

    public AudioManager audioManager;

    private void Start()
    {
        
        globalVolumeSlider.onValueChanged.AddListener(OnGlobalVolumeChanged);
        soundEffectVolumeSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
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

  
}
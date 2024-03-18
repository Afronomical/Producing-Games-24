using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingsRefSetter : MonoBehaviour
{


    void Start()
    {
        SettingsManager.Instance.FovEnable();
        SettingsManager.Instance.MotionBlurEnable();
        SettingsManager.Instance.ColourBlindnessEnable();
        SettingsManager.Instance.AudioEnable();
        SettingsManager.Instance.AntiAliasingEnable();
    }

   
}

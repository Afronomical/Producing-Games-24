using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSceneValues : MonoBehaviour
{
    public static SettingsSceneValues Instance;
    public float motionBlurNew;
    public float FovNew;
    public float volumeNew;
    public bool newNormal = false;
    public bool newProtanopia = false;
    public bool newDeuteranopia = false;
    public bool newTritanopia = false;

    public bool newAntiNone = false;
    public bool newAntiFXAA = false;
    public bool newAntiTAA = false;
    public bool newAntiSMAA = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        volumeNew = 0.5f;
        FovNew = 75f;
        motionBlurNew = 0f;
    }

    
  
}

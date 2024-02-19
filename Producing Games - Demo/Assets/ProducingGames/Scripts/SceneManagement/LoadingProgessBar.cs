using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LoadingProgessBar : MonoBehaviour
{
    private Image image; 
    
    private void Awake()
    {
        image = transform.GetComponent<Image>();    
    }

    private void Update()
    {
        if (image != null)
        {
            image.fillAmount = LevelManager.GetLoadingProgress(); 
        }
    }
}

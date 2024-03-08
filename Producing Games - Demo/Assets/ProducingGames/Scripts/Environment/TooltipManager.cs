using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public TextMeshProUGUI tooltipText;
    public Image tooltipImage;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(string text, Sprite image)
    {
        gameObject.SetActive(true);

        if(tooltipText != null)
        {
            tooltipText.text = text;
        }
        
        if(tooltipImage!= null)
        {
            tooltipImage.sprite = image;
        }
        
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
        tooltipImage.sprite = null;
    }
}

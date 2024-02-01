using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public TextMeshProUGUI tooltipText;
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

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
    }
}

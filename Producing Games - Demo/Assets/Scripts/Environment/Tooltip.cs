using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string message;

    private void Start()
    {
        gameObject.GetComponent<Outline>().enabled = false;

    }
    private void OnMouseEnter()
    {
        TooltipManager.Instance.ShowTooltip(message);
        gameObject.GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    { 
        TooltipManager.Instance.HideTooltip();
        gameObject.GetComponent<Outline>().enabled = false;
    }
}

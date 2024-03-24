using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashLightTooltipControl : MonoBehaviour
{
    public TMP_Text textBox;
    public string tooltip;

    private void Start()
    {
        textBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textBox.text = tooltip;
            textBox.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}

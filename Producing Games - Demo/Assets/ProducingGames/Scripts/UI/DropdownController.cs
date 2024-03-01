using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    // Reference to the TMP_Dropdown component
    TMP_Dropdown dropdown;

    void Start()
    {
        // Get the TMP_Dropdown component
        dropdown = GetComponent<TMP_Dropdown>();

        // Subscribe to the OnValueChanged event
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    // This method is called when the dropdown value changes
    void OnDropdownValueChanged(int index)
    {
        // Deselect all other options
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            // Set the state of each option based on whether it's the selected one or not
            dropdown.options[i].text = i == index ? "Option " + (i + 1) + " (Selected)" : "Option " + (i + 1);
        }
    }
}
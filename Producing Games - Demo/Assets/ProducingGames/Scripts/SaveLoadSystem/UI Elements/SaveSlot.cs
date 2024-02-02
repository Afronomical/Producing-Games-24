using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// Controls the actions of the save slot UI element
/// </summary>

public class SaveSlot : MonoBehaviour
{
    [Header("Profile:")]
    [SerializeField] private string profileID;

    [Header("Content:")]
    [SerializeField] private GameObject noData;
    [SerializeField] private GameObject hasData;
    [SerializeField] private TextMeshProUGUI profileIDNameText; // INFO: Doesn't have to be this, this is just an example
    [SerializeField] private TextMeshProUGUI clicksCompletedText; // INFO: Doesn't have to be this, this is just an example

    private Button saveSlotButton;

    public string GetProfileID() => profileID;

    public void SetInteractable(bool isInteractable) { saveSlotButton.interactable = isInteractable; }

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    /// <summary>
    /// Given that the provide data of the profile is null
    /// we can set the UI to show the slot as EMPTY, otherwise
    /// we can show it as having data
    /// </summary>
    /// <param name="data"></param>
    public void SetData(GameData data)
    {
        if (data == null)
        {
            noData.SetActive(true);
            hasData.SetActive(false);
        }
        else
        {
            noData.SetActive(false);
            hasData.SetActive(true);

            profileIDNameText.text = profileID;
            clicksCompletedText.text = "Clicks Completed: " + data.clicksCompleted;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// Controls the save slot menu
/// </summary>

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    //[SerializeField] private MainMenuController mainMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    /// <summary>
    /// Used in the on click event of the button UI component, the save slot
    /// provided is the same save slot that the button is a part of
    /// </summary>
    /// <param name="saveSlot"></param>
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        SaveLoadManager.Instance.ChangeCurrentProfileID(saveSlot.GetProfileID());

        if (!isLoadingGame)
        {
            SaveLoadManager.Instance.NewGame();
        }

        // INFO: The game is saved before transitioning to a new scene occurs
        SaveLoadManager.Instance.SaveGame();

        // INFO: Currently loads the default scene, this will need to be changed
        // based on whatever level we want the save to load to
        SceneManager.LoadSceneAsync("Testbed");
    }

    public void OnBackClicked()
    {
        //mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    /// <summary>
    /// Used for switching between the main and save slots menus
    /// </summary>
    /// <param name="isLoadingGame"></param>
    public void ActivateMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        // INFO: Get a dictionary that holds all the data of all the saved profiles
        Dictionary<string, GameData> profilesGameData = SaveLoadManager.Instance.GetAllProfilesGameData();

        // INFO: Loop through each save slot and set their contents
        foreach (SaveSlot saveSlot in saveSlots)
        {
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out GameData profileData);
            saveSlot.SetData(profileData);

            if (profileData == null && isLoadingGame)
                saveSlot.SetInteractable(false);
            else
                saveSlot.SetInteractable(true);
        }
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}

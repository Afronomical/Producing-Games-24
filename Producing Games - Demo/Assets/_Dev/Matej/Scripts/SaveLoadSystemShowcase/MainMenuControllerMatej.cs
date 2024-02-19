using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControllerMatej : MonoBehaviour
{
    [Header("Menu Navigation:")]
    [SerializeField] private SaveSlotsMenu saveSlotMenu;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        if (!SaveLoadManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotMenu.ActivateMenu(false);
        DeactivateMenu();
    }

    public void OnContinueClicked()
    {
        DisableButtonInteractions();

        SaveLoadManager.Instance.SaveGame();

        SceneManager.LoadSceneAsync("SaveLoadShowcaseSceneMatej");
    }

    public void OnLoadGameClicked()
    {
        saveSlotMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    private void DisableButtonInteractions()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}

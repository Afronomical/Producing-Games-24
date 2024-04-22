using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController_Matej : MonoBehaviour
{
    
    public void PlayGame()
    {
        LevelManager.LoadScene(LevelManager.Scenes.MainUpdated); 
    }

    public void OpenCredits()
    {
        LevelManager.LoadScene(LevelManager.Scenes.Credits);

    }

    public void OpenSettings()
    {
        LevelManager.LoadScene(LevelManager.Scenes.Settings);
       
    }

    public void PlayTutorial()
    {
        LevelManager.LoadScene(LevelManager.Scenes.NewTutorial);
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //add more if needed


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

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void OpenMenu()
    {
        LevelManager.LoadScene(LevelManager.Scenes.Menu);
       
    }

    public void OpenWin()
    {
        LevelManager.LoadScene(LevelManager.Scenes.WinScreen);
       
    }
    
    public void OpenLose()
    {
        LevelManager.LoadScene(LevelManager.Scenes.LoseScreen);
       
    }
}
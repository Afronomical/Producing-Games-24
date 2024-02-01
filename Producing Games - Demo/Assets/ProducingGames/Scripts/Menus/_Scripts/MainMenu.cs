using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        LevelManager.LoadScene(LevelManager.Scenes.Main); 
    }

    public void OpenSettings()
    {
        LevelManager.LoadScene(LevelManager.Scenes.Settings);
       
    }
}

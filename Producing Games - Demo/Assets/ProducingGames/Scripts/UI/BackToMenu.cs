using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Unpause time
        LevelManager.LoadScene(LevelManager.Scenes.Menu);
    }
}

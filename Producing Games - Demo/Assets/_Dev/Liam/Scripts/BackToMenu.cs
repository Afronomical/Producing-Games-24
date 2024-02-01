using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitToMainMenu()
    {

        Time.timeScale = 1f; // Unpause time
        LevelManager.LoadScene(LevelManager.Scenes.Menu);
    }
}

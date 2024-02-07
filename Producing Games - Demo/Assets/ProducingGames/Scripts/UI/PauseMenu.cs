using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsPanel;
    public bool isPaused = false;


    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Unpause time
        isPaused = false;

        // Show and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause time
        isPaused = true;

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSettings()
    {
        //LevelManager.LoadScene(LevelManager.Scenes.Settings);//
        settingsPanel.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        //LevelManager.LoadScene(LevelManager.Scenes.Settings);//
        settingsPanel.SetActive(false);
    }


    public void QuitToMainMenu()
    {
        
        Time.timeScale = 1f; // Unpause time
        LevelManager.LoadScene(LevelManager.Scenes.Menu);
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
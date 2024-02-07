using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsPanel;
    public bool isPaused = false;

    private CameraLook cameraLookScript;

    private void Start()
    {
        settingsPanel.SetActive(false);
        // Find the CameraLook script attached to the camera
        cameraLookScript = Camera.main.GetComponent<CameraLook>();
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

        // Enable the CameraLook script when resuming
        if (cameraLookScript != null)
            cameraLookScript.enabled = true;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause time
        isPaused = true;

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Disable the CameraLook script when pausing
        if (cameraLookScript != null)
            cameraLookScript.enabled = false;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseSettings()
    {
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
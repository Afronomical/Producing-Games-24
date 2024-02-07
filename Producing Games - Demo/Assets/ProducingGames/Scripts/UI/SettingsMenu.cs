using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SettingsMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject pauseMenu;
    public FPSCounter FpsCounter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    // Toggle FPS Display Button
    public void OnToggleFPSButtonClicked()
    {
        FpsCounter.ToggleFPSDisplay();
    }


    // Back Button
    public void OnBackButtonClicked()
    {
        // Toggle the visibility of the panels.
        settingsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }


}

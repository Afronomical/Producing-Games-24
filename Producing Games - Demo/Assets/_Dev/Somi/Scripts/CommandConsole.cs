using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Diagnostics;



public class CommandConsole : MonoBehaviour
{
    
    public static CommandConsole Instance;

    public TMP_InputField commandInput;
    
    public Dictionary<string, Action> commandsList = new Dictionary<string, Action>();



    public event Action InitializeEvents;

    #region CommandsList
    public event Action Commandcall;
    public event Action Restart;
    public event Action ToggleSprintStamina;
    public event Action ToggleFlashlight;
    public event Action IncrementTime;
    public event Action EndHour;
    public event Action Add1000Cash;

    #endregion
    CommandConsole() { }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        

        Commandcall += CommandTestCall;
        Restart += SceneRestart;
        
    }

    
    private void Start()
    {
        
        StartCoroutine(EventAwake());

        
        
        
    }
    public IEnumerator EventAwake()
    {
        
        yield return new WaitForSeconds(0.5f);
        commandsList.Add("commandCall", Commandcall);
        commandsList.Add("restart", Restart);
        commandsList.Add("togglestamina", ToggleSprintStamina);
        commandsList.Add("toggleflashlight", ToggleFlashlight);
        commandsList.Add("fastforward", IncrementTime);
        commandsList.Add("endhour", EndHour);
        commandsList.Add("addcash", Add1000Cash);
    }
    [STAThread]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            commandInput.gameObject.SetActive(!commandInput.gameObject.activeSelf);
            //GameManager.Instance.player.GetComponent<PlayerControls>().Disable();
        }

        if (Input.GetKeyDown(KeyCode.Return) && commandInput.gameObject.activeSelf == true)
        {
            CheckCommandInput(commandInput.text);
        }

        
        
        
    }
    public void CheckCommandInput(string input)
    {
        

        UnityEngine.Debug.Log("Command checking: " + input);
        foreach (var key in commandsList.Keys.ToList())
        {
            
            if (key == input)
            {
                UnityEngine.Debug.Log("Successfully ran command: " + key);
                commandsList[key]();
            }
            else
            {
              
            }
            commandInput.text = "";
        }
    }

    private void CommandTestCall()
    {
        UnityEngine.Debug.Log("It works");
    }

    private void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Command
{
    public string name;

    public Action command;
}
public class CommandConsole : MonoBehaviour
{
    
    public static CommandConsole Instance;

    public TMP_InputField commandInput;
    
    private Dictionary<string, Action> commandsList = new Dictionary<string, Action>();


    

    #region CommandsList
    public event Action Commandcall;
    public event Action Restart;
    public event Action ToggleSprintStamina;
    public event Action ToggleFlashlight;
    public event Action IncrementTime;
    public event Action EndHour;

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
        commandsList.Add("commandCall", Commandcall);
        commandsList.Add("restart", Restart);
        commandsList.Add("togglestamina", ToggleSprintStamina);
        commandsList.Add("toggleflashlight", ToggleFlashlight);
        commandsList.Add("fastforward", IncrementTime);
        commandsList.Add("endhour", EndHour);
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            commandInput.gameObject.SetActive(!commandInput.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Return) && commandInput.gameObject.activeSelf == true)
        {
            CheckCommandInput(commandInput.text);
        }
    }
    public void CheckCommandInput(string input)
    {
        Debug.Log("Commandcheck Called");
        foreach (var key in commandsList.Keys.ToList())
        {
            Debug.Log(key.ToString());
            Debug.Log(input);
            if (key == input)
            {
                
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
        Debug.Log("It works");
    }

    private void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

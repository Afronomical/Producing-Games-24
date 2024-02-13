using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandConsole : MonoBehaviour
{
    
    public static CommandConsole Instance;

    
    private Dictionary<string, Action> commands = new Dictionary<string, Action>();


    #region CommandsList
    public event Action Commandcall;
    public event Action Restart;

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

        commands.Add("Hi", Commandcall);
       // commands.Add("Restart", Restart));
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Restart?.Invoke();
            RandomPill.SetPlayerEffect(RandomPill.Effects.Slow);
            Debug.Log("ASdwqdaw");
        }
    }

    
}

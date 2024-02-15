using UnityEngine;
using System;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the events that happen in the game
/// </summary>

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public event Action FlickeringLightsEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        /*
         * if (RageMode is active)
         * {
         *      FlickeringLightsEvent?.Invoke();
         * }
         */

        // TEMP CODE FOR NOW
        FlickeringLightsEvent?.Invoke();
    }
}

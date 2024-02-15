using UnityEngine;
using System;

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
         * 
         * }
         */

        FlickeringLightsEvent?.Invoke();
    }
}

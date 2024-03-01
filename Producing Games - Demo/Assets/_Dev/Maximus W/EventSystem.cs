/*using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public BlinkingLightsEvent blinkingLightsEvent;
    public DistantScreamsEvent distantScreamsEvent;
    // Add references to other dynamic events

    public DemonicImageryEvent demonicImageryEvent;
    //public BloodWritingEvent bloodWritingEvent;
   // public PerverseIdolsEvent perverseIdolsEvent;
    // Add references to other idle events

    private void Start()
    {
        // Call methods to set up event triggers
        SetupDynamicEventTriggers();
        SetupIdleEventTriggers();
    }

    private void SetupDynamicEventTriggers()
    {
        // Attach dynamic event scripts to their respective triggers
        blinkingLightsEvent = GetComponent<BlinkingLightsEvent>();
        distantScreamsEvent = GetComponent<DistantScreamsEvent>();
        // Attach references to other dynamic events
    }

    private void SetupIdleEventTriggers()
    {
        // Attach idle event scripts to their respective triggers
        demonicImageryEvent = GetComponent<DemonicImageryEvent>();
        //bloodWritingEvent = GetComponent<BloodWritingEvent>();
        //perverseIdolsEvent = GetComponent<PerverseIdolsEvent>();
        // Attach references to other idle events
    }

    // Call this method to trigger dynamic events
    public void TriggerDynamicEvent(string eventName)
    {
        // Implement logic to call the specific dynamic event based on the event name
        switch (eventName)
        {
            case "BlinkingLights":
                blinkingLightsEvent.TriggerBlinkingLights();
                break;
            case "DistantScreams":
                distantScreamsEvent.TriggerDistantScreams();
                break;
            // Add cases for other dynamic events
            default:
                Debug.LogWarning("Unknown dynamic event: " + eventName);
                break;
        }
    }

    // Call this method to trigger idle events
    public void TriggerIdleEvent(string eventName)
    {
        // Implement logic to call the specific idle event based on the event name
        switch (eventName)
        {
            case "DemonicImagery":
                demonicImageryEvent.TriggerDemonicImagery();
                break;
            case "BloodWriting":
                bloodWritingEvent.TriggerBloodWriting();
                break;
            // Add cases for other idle events
            default:
                Debug.LogWarning("Unknown idle event: " + eventName);
                break;
        }
    }
}*/
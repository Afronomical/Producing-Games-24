using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 enlargedScale = new Vector3(0.3f, 0.3f, 0.3f);

    private void Start()
    {
        originalScale = new Vector3(3f, 3f, 1f);

        // Attach the HoverEffect to all child buttons
        Button[] childButtons = GetComponentsInChildren<Button>();
        foreach (Button button in childButtons)
        {
            AddHoverEffect(button);
        }
    }

    private void AddHoverEffect(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, (data) => OnPointerEnter(button));
        AddEventTriggerListener(trigger, EventTriggerType.PointerExit, (data) => OnPointerExit(button));
    }

    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    private void OnPointerEnter(Button button)
    {
        button.transform.localScale += enlargedScale;
    }

    private void OnPointerExit(Button button)
    {
        button.transform.localScale = originalScale;
    }
}
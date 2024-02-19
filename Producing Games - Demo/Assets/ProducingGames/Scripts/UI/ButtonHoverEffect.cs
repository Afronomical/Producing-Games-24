using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        // Attach the HoverEffect to all child buttons
        Button[] childButtons = GetComponentsInChildren<Button>();
        foreach (Button button in childButtons)
        {
            AddHoverEffect(button);
        }
    }

    private void AddHoverEffect(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        Vector3 enlargedScale = originalScale + new Vector3(0.4f, 0.4f, 0f);

        // Check if the button already has an EventTrigger component
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            // If not, add a new one
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Remove existing EventTrigger entries to avoid duplicates
        trigger.triggers.Clear();

        // Add new EventTrigger entries
        AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, (data) => OnPointerEnter(rectTransform, enlargedScale));
        AddEventTriggerListener(trigger, EventTriggerType.PointerExit, (data) => OnPointerExit(rectTransform, originalScale));
        AddEventTriggerListener(trigger, EventTriggerType.PointerClick, (data) => OnPointerClick(rectTransform));
    }

    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    private void OnPointerEnter(RectTransform rectTransform, Vector3 targetScale)
    {
        rectTransform.localScale = targetScale;
    }

    private void OnPointerExit(RectTransform rectTransform, Vector3 originalScale)
    {
        rectTransform.localScale = originalScale;
    }

    private void OnPointerClick(RectTransform rectTransform)
    {
        // Disable the hover effect when the button is clicked
        DisableHoverEffect(rectTransform);
    }

    private void DisableHoverEffect(RectTransform rectTransform)
    {
        // Reset the scale to the original size
        rectTransform.localScale = originalScale;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PagerIcon : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI expandedClockText;
    // Public variable for time speed multiplier
    public float timeSpeedMultiplier = 24f / 3f; // Completes a 24-hour cycle in 3 minutes

    public bool is24HourMode = false; // If Bool = true then the clock will do a full 24Hour period, If False the clock will go from 8pm to 8am

    private void Update()
    {
        // Calculate fake time
        System.DateTime fakeTime = CalculateFakeTime();

        // Extract hours, minutes, and AM/PM
        int hours = fakeTime.Hour % (is24HourMode ? 24 : 12);
        int minutes = fakeTime.Minute;
        string amPm = is24HourMode ? "" : (fakeTime.Hour >= 12) ? "PM" : "AM";

        // Adjust 0 hours to 12
        hours = hours == 0 ? (is24HourMode ? 0 : 12) : hours;

        // Update the TextMeshProUGUI element
        clockText.text = $"{hours:D2}:{minutes:D2} {amPm}";
        expandedClockText.text = $"{hours:D2}:{minutes:D2} {amPm}";
    }

    private System.DateTime CalculateFakeTime()
    {
        float elapsedMinutes = Time.time * timeSpeedMultiplier;

        // Calculate fake time based on user-selected mode
        if (is24HourMode)
        {
            return System.DateTime.Now.AddMinutes(elapsedMinutes);
        }
        else
        {
            System.DateTime baseTime = System.DateTime.Today.AddHours(20); // 20 = 8pm
            float elapsedHours = elapsedMinutes / 60f;
            System.DateTime fakeTime = baseTime.AddHours(elapsedHours % 12); // 12-hour cycle from 8 PM to 8 AM

            // Handle AM/PM transition at midnight
            if (fakeTime.Hour == 12)
            {
                fakeTime = fakeTime.AddHours(12); // Add 12 hours to switch from AM to PM at midnight
            }

            return fakeTime;
        }
    }
}
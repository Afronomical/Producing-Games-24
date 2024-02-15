using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 30.0f; // Adjust the scroll speed as needed
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the anchored position of the RectTransform
        rectTransform.anchoredPosition += new Vector2(0f, -scrollSpeed * Time.deltaTime);

        // can add additional logic to stop the scrolling or trigger other events
        // like stop scrolling after a certain time or when a key is pressed
    }
}

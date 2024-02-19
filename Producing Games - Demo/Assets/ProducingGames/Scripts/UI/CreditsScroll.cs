using System.Collections;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float initialDelay = 3.0f; // Delay before the credits start scrolling
    public float scrollSpeed = 30.0f;
    public float stopDelay = 5.0f; // Time to wait after reaching the end before stopping
    public KeyCode stopKey = KeyCode.Space; // Key to stop the scrolling

    private RectTransform rectTransform;
    private bool isScrolling = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(StartScrolling());
    }

    IEnumerator StartScrolling()
    {
        yield return new WaitForSeconds(initialDelay);
        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling)
            return;

        rectTransform.anchoredPosition += new Vector2(0f, -scrollSpeed * Time.deltaTime);

        // Stop scrolling if the specified key is pressed
        if (Input.GetKeyDown(stopKey))
            StopScrolling();

        // Add additional logic to stop scrolling after a certain time
        // or when reaching a specific position
        if (rectTransform.anchoredPosition.y <= -500.0f) // Adjust the threshold as needed
            StopScrolling();
    }

    void StopScrolling()
    {
        isScrolling = false;
        StartCoroutine(StopAfterDelay());
    }

    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(stopDelay);
        // Add any additional logic or events to trigger after stopping
        Debug.Log("Credits stopped scrolling");
    }
}

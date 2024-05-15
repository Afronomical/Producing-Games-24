using System.Collections;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float initialDelay = 3.0f; // Delay before the credits start scrolling
    public float scrollSpeed = 30.0f;
    public float stopDelay = 5.0f; // Time to wait after reaching the end before stopping
    public KeyCode stopKey = KeyCode.Space; // Key to stop the scrolling
    public KeyCode speedKey = KeyCode.Mouse0;
    public float speedAccelRate = 2;

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

        if (Input.GetKey(speedKey))
        {
            rectTransform.anchoredPosition += new Vector2(0f, -scrollSpeed * Time.deltaTime * speedAccelRate);
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(0f, -scrollSpeed * Time.deltaTime);
        }



        // Add additional logic to stop scrolling after a certain time
        // or when reaching a specific position
        if (rectTransform.anchoredPosition.y >= 4200.0f) // Adjust the threshold as needed
            GetComponent<BackToMenu>().QuitToMainMenu();
    }


}

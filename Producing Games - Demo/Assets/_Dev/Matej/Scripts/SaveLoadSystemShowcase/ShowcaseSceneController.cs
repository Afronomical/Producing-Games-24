using TMPro;
using UnityEngine;


// INFO: Temp class for testing with UI
public class ShowcaseSceneController : MonoBehaviour
{
    [SerializeField] private TestClick testClickScript;
    [SerializeField] private TMP_Text clickText;

    private void Update()
    {
        clickText.text = "Clicks: " + testClickScript.GetMouseClicks().ToString();
    }
}

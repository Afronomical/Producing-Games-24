using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsEnd : MonoBehaviour
{
    private float creditsTime = 94f;

    private void Start()
    {
        StartCoroutine(SwitchScene());
    }

    private void Update()
    {
        SwitchSceneSkip();
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(creditsTime);
        SceneManager.LoadScene("Menu");
    }

    private void SwitchSceneSkip()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

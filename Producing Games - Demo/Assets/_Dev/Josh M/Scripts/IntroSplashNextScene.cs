using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSplashNextScene : MonoBehaviour
{
    private float introSplashTime = 7.5f;

    private void Start()
    {
        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(introSplashTime);
        SceneManager.LoadScene("Menu");
    }
}

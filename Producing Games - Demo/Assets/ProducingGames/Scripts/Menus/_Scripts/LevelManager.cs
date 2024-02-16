using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Security.Cryptography;

/// <summary>
/// Written by: Matthew Brake
/// Moderated by: Matej Cincibus.
/// 
/// 
/// Manages the transitions between scenes. Ensures that strings aren't misspelled when loading scene.
/// </summary>

public static class LevelManager
{
    private class LoadingMonoBehaviour : MonoBehaviour {  }


    /// <summary>
    /// Scenes of the game. Each represents a different level. 
    /// </summary>
    public enum Scenes
    {
        Loading,
        Tutorial,
        Settings,
        Main,
        Credits,
        Menu,
        WinScreen,
        LoseScreen
    }

    private static Action onLoaderCallBack;
    private static AsyncOperation asyncOperation;
    private static Scenes currentScene; 

    [SerializeField] private static List<Scenes> sceneTypes = new();
    [SerializeField] private static List<string> sceneNames = new();

    private static Dictionary<Scenes, string> sceneLibrary = new();

    private static void Start()
    {
        CommandConsole.Instance.Restart += RestartLevel;
        for (int i = 0; i < sceneNames.Count; i++)
        {
            sceneLibrary.Add(sceneTypes[i], sceneNames[i]);
        }

        LoadScene(currentScene);
    }

    /// <summary>
    /// Loads a desired scene based on our pre-set Scene Enums.
    /// See "Level Manager" Enums to determine desired scene. 
    /// 
    /// </summary>
    /// <param name="scene">This is the chosen scene based on set of pre defined Enums</param>
    public static void LoadScene(Scenes scene)
    {
            
            onLoaderCallBack = () =>
            {
                ///sets the loader callback to load the target scene 
                GameObject loaderGameObject = new GameObject("Loading Game Object"); 
                loaderGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
                SceneManager.LoadSceneAsync(scene.ToString());
                Debug.Log("Loaded Scene" + scene.ToString());
            };

            SceneManager.LoadScene(Scenes.Loading.ToString());

        
       
    }

    private static IEnumerator LoadSceneAsync(Scenes scene)
    {
        yield return null; 
        asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while(!asyncOperation.isDone)
        {
            yield return null;
            currentScene = scene; 
        }
    }
        

    /// <summary>
    /// Retrieves the amount of progress completed by loading bar. 
    /// </summary>
    /// <returns></returns>
    public static float GetLoadingProgress()
    {
        if(asyncOperation != null)
        {
            return asyncOperation.progress; 
        }
        else
        {
            return 1f; 
        }
    }

    public static void LoaderCallBack()
    {
       //execute loader callback which loads target scene 
        
        if(onLoaderCallBack != null)
        {
            onLoaderCallBack();
            onLoaderCallBack = null; 
        }
    }


    /// <summary>
    /// Restarts the current level from start. 
    /// </summary>
    /// <param name="scenes"></param>
    public static void RestartLevel()
    {
        LoadScene(currentScene); 
    }

    public static void NextLevel(Scenes scenes)
    {
        ///might be redundant 
    }


    ////functionality to get reference to game manager so can get state from the game and work out won/lost, and determine next steps 
}

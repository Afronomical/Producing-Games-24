using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by: Matthew Brake
/// 
/// 
/// Manages the loading screen and informing the scene manager when it has completed load. 
/// </summary>
public class LoadingCallBack : MonoBehaviour
{
    private bool isFirstUpdate = true;


    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            LevelManager.LoaderCallBack(); 
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

/// <summary>
/// Written By: Matthew Brake.
/// Moderated By: Matej Cincibus
/// 
/// 
/// .Manages the game state and informs the level manager if level needs changing 
/// </summary>

public class GameManager: MonoBehaviour
{
   public static GameManager Instance;

    public static event Action<GameState> OnGameStateChanged;

    //// player reference ********** 
    //// AI references. 
    ///all for information on their health,sanity etc for use by UI 

    public GameState State;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ///updategamestate to menu 
        ///
        #if UNITY_STANDALONE_WIN
        UpdateGameState(GameState.Menu);
#endif 
    }
   
    /// <summary>
    /// Changes the current state of the game, depending on factors within gameplay. 
    /// </summary>
    /// <param name="NewState"></param>
    public void UpdateGameState(GameState NewState)
    {
        State = NewState;

        switch (NewState)
        {
            case GameState.Menu:
                break;
            case GameState.GameStart:
                break;
            case GameState.NPC_Possessed:
                break; 
            case GameState.PlayerWin:
                break;
            case GameState.PlayerLose:
                break;
        }

        OnGameStateChanged?.Invoke(NewState); 
        ////tells any subscribed functions/classes that the state has changed 
    }


    public enum GameState
    {
        Menu,
        GameStart,
        NPC_Possessed,
        PlayerWin,
        PlayerLose

        /////template states, to be changed 
    }


    //public static GameState State { get; private set; }

    ///
    public static void EndGame()
    {
        ////if playerstate == win 
        ///win game 
        ///levelmanager.endgame or roll credits 
        ///
        ///if playerstate == lose 
        ///lose game. main menu or restart level 
    }

}

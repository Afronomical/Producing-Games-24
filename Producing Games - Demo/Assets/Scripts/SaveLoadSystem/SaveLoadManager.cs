using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// Singleton design, the point from which all data is saved to and loaded from
/// </summary>

public class SaveLoadManager : MonoBehaviour
{
    [Header("Debugging Settings:")]
    [SerializeField] private bool isSavingEnabled = true;
    [SerializeField] private bool initializeData = false;
    [SerializeField] private bool overrideCurrentProfileID = false;
    [SerializeField] private string newCurrentProfileID = "";

    [Header("File Configuration Settings:")]
    [SerializeField] private string fileName;

    [Header("Auto Saving Configuration Settings:")]
    [SerializeField] private float autosaveIntervalSeconds = 60f;

    private FileDataHandler fileDataHandler = null;
    private GameData gameData;
    private List<ISaveable> saveableObjects = new();
    private Coroutine autosaveCoroutine;

    private string currentProfileID = "";

    /// <summary>
    /// Used for UI, using this we can potentially prevent the player from being
    /// able to press specific buttons e.g. (Continue Button) when there is no data
    /// that can be used to continue a game
    /// </summary>
    /// <returns></returns>
    public bool HasGameData() => gameData != null;

    /// <summary>
    /// Returns a dictionary that contains the names and data of all the profiles
    /// we currently have
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GameData> GetAllProfilesGameData() => fileDataHandler.LoadAllProfiles();

    public static SaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        // INFO: Given that another instance of the class is found the newer
        // SaveLoadManager (gameObject) is destroy to ensure there is only one
        if (Instance != null)
        {
            Debug.LogWarning("More than one SaveLoadManager found in the scene! Destroying the newest one!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // INFO: SaveLoadManager needs to persist through scenes so that the
        // ability to save/load data isn't lost at any point in the game
        DontDestroyOnLoad(gameObject);

        // INFO: Debugging feature, in case you don't want your playthrough
        // to be saved whilst testing something e.g. (New feature etc.)
        if (!isSavingEnabled)
            Debug.LogWarning("Saving is not enabled!");

        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        currentProfileID = fileDataHandler.GetMostRecentlyUpdatedProfileID();

        if (overrideCurrentProfileID)
        {
            currentProfileID = newCurrentProfileID;
            Debug.LogWarning("The current profile ID is being overriden with a new profile ID: " + newCurrentProfileID);
        }
    }

    private void OnEnable()
    {
        // INFO: OnSceneLoaded is subscribed to the sceneLoaded event upon the
        // object becoming enabled so that it is called whenever a sceneLoaded is fired
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // INFO: OnSceneLoaded is unsubscribed from the sceneLoaded event upon
        // the object becoming disabled for cleanup purposes, so that it doesn't
        // get accidentaly called when we no longer want it to
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        // INFO: Saves the game automatically anytime the game is exited, unsure if
        // this is a feature we want, but it's here for now
        SaveGame();
    }

    /// <summary>
    /// Upon a scene being loaded we ensure we retrieve all objects that inherit
    /// the ISaveable interface and then call LoadGame so that the ISaveable
    /// objects can be loaded with their respective data
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        saveableObjects = FindAllSaveableObjects();
        LoadGame();

        // INFO: Given that an auto-save coroutine is already running from before
        // we can stop it and start it again from the start
        if (autosaveCoroutine != null)
            StopCoroutine(autosaveCoroutine);

        autosaveCoroutine = StartCoroutine(AutoSave());
    }

    /// <summary>
    /// Creates a GameData instance which is initialized with default values
    /// </summary>
    public void NewGame()
    {
        gameData = new GameData();
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    public void LoadGame()
    {
        // INFO: If saving isn't enabled, we know that there is no external data stored
        // so we can return right away
        if (!isSavingEnabled)
            return;

        // INFO: Load data from saved file, if no data is found the object
        // will be set to null, which in turn will create a new game
        gameData = fileDataHandler.Load(currentProfileID);

        // INFO: In the case of debugging where we don't want to go through the
        // primary scene in-charge of loading/creating a new game (main menu)
        // and we just want to go into a random scene and start testing, we can
        // set bInitializeData to true in the scene so that a new game is
        // automatically created
        if (gameData == null && initializeData)
            NewGame();

        // INFO: If no saved data is found then we can't continue with loading
        if (gameData == null)
        {
            Debug.Log("No save files found, a new game must be created!");
            return;
        }

        // INFO: Load game data for each saveable object, using the game data
        // we have retrieved from the external file
        foreach (ISaveable obj in saveableObjects)
        {
            obj.LoadData(gameData);
        }
    }

    /// <summary>
    /// Saves the game
    /// </summary>
    public void SaveGame()
    {
        // INFO: If saving isn't enabled, then we don't need to save
        // so we return right away
        if (!isSavingEnabled)
            return;

        // INFO: If we don't have data to save then do not continue
        if (gameData == null)
        {
            Debug.LogWarning("No data found, a new game needs to be started before data can be saved.");
            return;
        }

        // INFO: Save game data for each saveable object onto the provided
        // game data object which will then be serialized and put onto the
        // external file
        foreach (ISaveable obj in saveableObjects)
        {
            obj.SaveData(gameData);
        }

        // INFO: Update the time at which the data was saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // INFO: Save the gameData onto the external file
        fileDataHandler.Save(gameData, currentProfileID);
    }

    /// <summary>
    /// Coroutine which runs every N seconds, automatically saves the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoSave()
    {
        // INFO: While (true) ensures the autosave coroutine repeats until
        // the end of the program
        while (true)
        {
            yield return new WaitForSeconds(autosaveIntervalSeconds);
            SaveGame();
            Debug.Log("Auto Save occured!");
        }
    }

    /// <summary>
    /// Goes through all objects (active/inactive) located in the scene we are
    /// currently in and makes a note of any object that is of type ISaveable
    /// and returns them all in the form of a list
    /// </summary>
    /// <returns></returns>
    private List<ISaveable> FindAllSaveableObjects()
    {
        IEnumerable<ISaveable> saveableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();
        return new List<ISaveable>(saveableObjects);
    }

    /// <summary>
    /// Used to change what profile we are currently loading data from
    /// upong changing the game is reloaded with the data held at the
    /// new profile location
    /// </summary>
    /// <param name="newProfileID">The profile we want to use instead</param>
    public void ChangeCurrentProfileID(string newProfileID)
    {
        currentProfileID = newProfileID;
        LoadGame();
    }
}

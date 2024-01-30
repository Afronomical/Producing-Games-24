using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// Handles the saving and loading of data to and from an external file
/// </summary>

public class FileDataHandler
{
    private string dataDirectoryPath;
    private string dataFileName;
    private readonly string backupExtension = ".bak";

    /// <summary>
    /// Class constructor requires the specifying of the directory path as well
    /// as the name of the file used to store the game data
    /// </summary>
    /// <param name="dataDirectoryPath">The filepath of where the data is stored, best to use: Application.persistentDataPath</param>
    /// <param name="dataFileName">The name of file in which the data is stored</param>
    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    /// <summary>
    /// Loads the contents of the file held at the specified profileID and returns it
    /// </summary>
    /// <param name="profileID">What profile are we loading the data from</param>
    /// <param name="allowRestoreFromBackup">Attempt loading from backup file if original is corrupt</param>
    /// <returns></returns>
    public GameData Load(string profileID, bool allowRestoreFromBackup = true)
    {
        // INFO: If a profile ID is not provided we can immediately return
        if (profileID == null)
            return null;

        // INFO: Filepath where the saved data is stored
        string fullPath = Path.Combine(dataDirectoryPath, profileID, dataFileName);

        GameData loadedData = null;

        // INFO: Prevents data from being loaded if a save file doesn't exist
        if (File.Exists(fullPath))
        {
            // INFO: Tries to load the data onto the temporary loadedData variable,
            // will catch any errors that occur during the process
            try
            {
                loadedData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(fullPath));
            }
            catch (Exception e)
            {
                // INFO: Given that we have the system setup to restore from a backup file
                // if we initially fail to load the data from the specified fullpath, we can then
                // try to load data from the backup filepath
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    // INFO: Given that we are successful in retrieving a backup file we can then
                    // try to load this file
                    bool rollbackSuccess = AttemptRollback(fullPath);

                    // INFO: Ensures we only attempt to load the backup file once, if the loading of
                    // backup file fails we won't have to worry about being stuck in an infinite loop
                    // since we were able to retrieve the backup file above, but weren't successful in
                    // loading its contents
                    if (rollbackSuccess)
                        loadedData = Load(profileID, false);
                }
                else
                    Debug.LogError("Error occured when trying to load file at path: " + fullPath + " and backup didn't work.\n" + e);
            }
        }

        return loadedData;
    }

    /// <summary>
    /// Saves the data onto an external file
    /// </summary>
    /// <param name="data">All the data held about the current playthrough</param>
    /// <param name="profileID">The specified profile to which we are saving the data to</param>
    /// <exception cref="Exception"></exception>
    public void Save(GameData data, string profileID)
    {
        // INFO: If a profile ID is not provided we can immediately return
        if (profileID == null)
            return;

        // INFO: Filepath where the saved data is stored as well as the filepath
        // where the backup file is located
        string fullPath = Path.Combine(dataDirectoryPath, profileID, dataFileName);
        string backupFilePath = fullPath + backupExtension;

        // INFO: In case the folder where saved data is stored is not found, the code below will
        // attempt to create the folder at the specified path, however if it fails, it will be
        // caught and won't create errors
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }

        // INFO: Write all the data held by the GameData class into the file located at the fullPath
        // Indented formatting just makes the data easier to read
        File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));

        // INFO: Verify that newly saved file can be loaded
        GameData verifiedGameData = Load(profileID);

        // INFO: Given that the data can be loaded (isn't null) we can use it to create a backup
        // file in case the original data file becomes corrupt for some reason, this ensures the
        // player doesn't lose all their progress
        if (verifiedGameData != null)
            File.Copy(fullPath, backupFilePath, true);
        else
            throw new Exception("Save filed couldn't be verified and backup couldn't be created!");
    }

    /// <summary>
    /// Returns a dictionary of all the profiles we have, the key is used to specify the
    /// name of the profile and the value is the data held by that particular profile
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new();

        // INFO: Goes through all directory names found in the specified path
        IEnumerable<DirectoryInfo> directoryInfoList = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();

        foreach (DirectoryInfo directoryInfo in directoryInfoList)
        {
            string profileID = directoryInfo.Name;
            string fullPath = Path.Combine(dataDirectoryPath, profileID, dataFileName);

            // INFO: Checks whether the file used to hold the save data exists in
            // the file path specified if not we skip over it as there is no point
            // in loading a blank profile
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping this folder as there is no save data found inside of it: " + profileID);
                continue;
            }

            // INFO: Load data from the specified filepath
            GameData profileData = Load(profileID);

            // INFO: Security check prevents the saving of null data in case
            // anything goes wrong during the loading of the data
            if (profileData == null)
                Debug.LogError("Something went wrong with loading the data. ProfileID: " + profileID);
            else
                profileDictionary.Add(profileID, profileData);
        }

        return profileDictionary;
    }

    /// <summary>
    /// Retrieves the most recently saved profileID, this is primarily useful if we have
    /// something like a Play/Continue button, as this will ensure the player is able
    /// to play from their latest save
    /// </summary>
    /// <returns></returns>
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        // INFO: Loads all the profiles we have
        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();

        // INFO: We go through each entry in the profiles Dictionary and compare the
        // time at which it was created
        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            // INFO: Skips over any null data (Extra security measure)
            if (gameData == null)
                continue;

            // INFO: First piece of data that holds values is automatically assigned
            // as mostRecentProfileID so that it can be compared with the data held
            // in the other IDs (If there are other data files)
            if (mostRecentProfileID == null)
                mostRecentProfileID = profileID;
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                // INFO: Compares the times, whichever is greater is the more recent
                if (newDateTime > mostRecentDateTime)
                    mostRecentProfileID = profileID;
            }
        }
        return mostRecentProfileID;
    }

    /// <summary>
    /// Attempts to retrieve the data held on the backup file and copy it over onto
    /// the primary save file, which has become corrupted in some way
    /// </summary>
    /// <param name="fullPath">The path of the primary save file where the data is held</param>
    /// <returns></returns>
    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;

        // INFO: Firstly checks whether the file exists at the provided filepath,
        // given that it does it copies it's contents onto the file held at the
        // fullPath which is likely to have been corrupted and sets success to true
        // indicating we were able to rollback the corrupted data with the backup data
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            else
                throw new Exception("Tried to roll back, but no backup file exists to roll back to!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to the backup file at: " + backupFilePath + "\n" + e);
        }

        return success;
    }
}

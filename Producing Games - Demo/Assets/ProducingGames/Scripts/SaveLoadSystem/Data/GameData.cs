using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// Stores all game data we want saved e.g. (Entity positions, Inventory contents,
/// etc.).
///
/// The following data types have been changed to types that can be serializable:
/// 1. Vector3 -> SerializableVector3
/// </summary>

/*
 * For saving things like lists, arrays or dictionaries, in which you have a lot 
 * of repeating objects, for example a dictionary of coins where your key is the 
 * unique ID of each coin and the value is a bool which is used to identify whether 
 * the coin has been picked up or not, we will need to use: 
 * System.Guid.NewGuid().ToString() 
 * to generate a unique ID for each coin, so that we can identify them and load
 * them with the appropriate data when loading.
*/

// INFO: Any class that has data you want saved must be marked as System.Serializable
[System.Serializable]
public class GameData
{
    // INFO: Time-based system, everytime the data is saved the time of saving is
    // recorded, so we can keep track of the latest save
    public long lastUpdated;

    // INFO: Example Data
    public int clicksCompleted;
    public SerializableVector3 playerPosition;
    /*
    public Vector3 playerPosition; <- Won't work as not serializable data type
    public SerializableVector3 playerPosition <- Will work when saving

    public Dictionary<string, Vector3> patientPositions;
    public Dictionary<string, bool> itemsCollected;
    */

    /// <summary>
    /// Class constructor used to initialize the default values of the data held
    /// within it
    /// </summary>
    public GameData()
    {
        // TEMP CODE
        clicksCompleted = 0;
        playerPosition = new SerializableVector3(new Vector3(0, 2, -5)); // Starting position of player during every new game
    }
}
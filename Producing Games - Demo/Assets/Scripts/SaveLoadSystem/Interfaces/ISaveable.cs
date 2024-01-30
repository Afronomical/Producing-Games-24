/// <summary>
/// Written by: Matej Cincibus
/// 
/// Interface used to specify which classes hold data that we want
/// saved to the external files. Any class that has data that we want
/// saved must implement this interface and the defined methods within it
/// </summary>

public interface ISaveable
{
    /// <summary>
    /// Loads the provided data back into the class that inherits from
    /// the ISaveable interface
    /// </summary>
    /// <param name="data">Passed in by the SaveLoadManager</param>
    void LoadData(GameData data);

    /// <summary>
    /// Saves the provided data onto an external file
    /// </summary>
    /// <param name="data">Passed in by the SaveLoadManager</param>
    void SaveData(GameData data);
}

using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// 
/// The serializable version of the Vector3 data type, by default the JSON serialization
/// is unable to save unity specify data types, which is why we had to make our own
/// implementation of the Vector3 class
/// </summary>

// INFO: Any class that has data you want saved must be marked as System.Serializable
[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    /// <summary>
    /// Converts a unity Vector3 to a serializable Vector3
    /// </summary>
    /// <param name="vector3"></param>
    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    /// <summary>
    /// Converts a serializable Vector3 back to a unity Vector3
    /// </summary>
    /// <returns></returns>
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour, ISaveable
{
    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition.ToVector3();
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = new SerializableVector3(transform.position);
    }
}
